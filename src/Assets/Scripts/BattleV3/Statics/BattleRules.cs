using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BattleV3;
using UnityEngine;

namespace BattleV3
{
    public static class BattleRules
    {

        private static float NPCBattlersBaseCounterProbability => 0.1f;
        private static float NPCBattlersMinimumCounterProbability = 0.0025f;
        private static float NPCBattlersMaxCounterProbability = 1f;

        public static bool LetTargetAttemptToReact(Battler user, Battler target, AbilityV2 ability)
        {
            var solvedSpd = ability.Resource switch
            {
                StatV2.EResourceType.Health => user.BattlerStats.PhyDef.CurrentValue -
                                              target.BattlerStats.PhyDef.CurrentValue,
                StatV2.EResourceType.Stamina => user.BattlerStats.PhySpd.CurrentValue -
                                                target.BattlerStats.PhySpd.CurrentValue,
                StatV2.EResourceType.Mana => user.BattlerStats.MagSpd.CurrentValue -
                                             target.BattlerStats.MagSpd.CurrentValue,
                _ => 0f
            };

            return UnityEngine.Random.Range(0f, 1f) < Mathf.Clamp(NPCBattlersBaseCounterProbability + (solvedSpd/100), NPCBattlersMinimumCounterProbability,NPCBattlersMaxCounterProbability ) ;
        }


        
        public static bool ResolveInterruptProbability(Battler target, ReactionV2 getCurrentReaction)
        {
            //todo: Real formula
            return true;
        }

        public static Dictionary<EffectV2, AbilityPower> GetEffectsBasePower(AbilityV2 ability, Battler user)
        {
            var effectModifiersUser = new Dictionary<EffectV2, AbilityPower>(ability.EffectPowerDict.Keys.Count);
            foreach (var effect in ability.EffectPowerDict.Keys)
            {
                var abilityPower = new AbilityPower();
                var basePower = user.BattlerStats.GetBaseEffectPower(effect);
                if (user.BattlerStats.CheckForCritical(ability.Resource))
                {
                    basePower += user.BattlerStats.GetCriticalHitPowerModifier(ability.Resource);
                    abilityPower.UserPower = basePower;
                    abilityPower.IsCriticalHit = true;
                    effectModifiersUser.Add(effect, abilityPower);
                }
                else
                {
                    effectModifiersUser.Add(effect, abilityPower);
                }
            }
            return effectModifiersUser;
        }

        public static Dictionary<EffectV2, AbilityPower> ApplyReactionsModifiersIncomingAll(AbilityV2 ability, List<IMonoReactions> triggeredReactions, Dictionary<EffectV2, AbilityPower> effectModifiersAllTargets)
        {
            foreach (var triggeredReaction in triggeredReactions)
            {
                if (triggeredReaction.ReactionHasType(ReactionV2.Type.ModifyIncomingEffectAll))
                {
                    foreach (var effect in ability.EffectPowerDict.Keys)
                    {
                        if (triggeredReaction.GetReaction().IsMatch(effect))
                        {
                            var reactionModifier = triggeredReaction.GetReactionModifiers();
                            
                            if (effectModifiersAllTargets.ContainsKey(effect))
                            {
                                if (effectModifiersAllTargets[effect].BestNegativeReactionModifier > reactionModifier)
                                    effectModifiersAllTargets[effect].BestNegativeReactionModifier = reactionModifier;
                                else if (effectModifiersAllTargets[effect].BestPositiveReactionModifier < reactionModifier)
                                    effectModifiersAllTargets[effect].BestPositiveReactionModifier = reactionModifier;
                            }
                            else
                            {
                                var power = new AbilityPower();
                                if (reactionModifier > 0)
                                {
                                    power.BestPositiveReactionModifier = reactionModifier;
                                    effectModifiersAllTargets.Add(effect, power);
                                }
                                else if (reactionModifier < 0)
                                {
                                    power.BestNegativeReactionModifier = reactionModifier;
                                    effectModifiersAllTargets.Add(effect, power);
                                }
                                else
                                {
                                    effectModifiersAllTargets.Add(effect, power);
                                }

                            }
                        }
                    }
                }
            }

            return effectModifiersAllTargets;
        }

        public static Dictionary<Battler, Dictionary<EffectV2, AbilityPower>> AssignEffectsToBattlers(Dictionary<EffectV2, AbilityPower> effectModifiersAllTargets, Battler user, Battler primaryTarget)
        {
            var effectBattlerAssignments = new Dictionary<Battler, Dictionary<EffectV2, BattleRules.AbilityPower>>();
            foreach (var battler in Battle.GetBattlers())
            {
                Dictionary<EffectV2, BattleRules.AbilityPower> powerDic;
                if (primaryTarget && battler == primaryTarget)
                {
                    powerDic = effectModifiersAllTargets
                        .Where(kv => kv.Key.targettingType is EffectV2.ETargettingType.PrimaryTarget or EffectV2.ETargettingType.AllExcludeUser)
                        .ToDictionary(kv => kv.Key, kv => kv.Value);
                }
                else if (battler == user)
                {
                    powerDic = effectModifiersAllTargets
                        .Where(kv => kv.Key.targettingType is EffectV2.ETargettingType.User )
                        .ToDictionary(kv => kv.Key, kv => kv.Value);
                }
                else
                {
                    powerDic = effectModifiersAllTargets
                        .Where(kv => kv.Key.targettingType is EffectV2.ETargettingType.AllExcludeUser or EffectV2.ETargettingType.AllExcludeUserAndPrimary )
                        .ToDictionary(kv => kv.Key, kv => kv.Value);
                }
                
                if(powerDic.Count > 0)
                    effectBattlerAssignments.Add(battler, powerDic);
            }

            return effectBattlerAssignments;
        }
        
        

        public static Dictionary<Battler, Dictionary<EffectV2, AbilityPower>> ApplyReactionsModifiersIncomingSelf(AbilityV2 ability, Dictionary<Battler, Dictionary<EffectV2, AbilityPower>> effectModifiersSelf, List<IMonoReactions> triggeredReactions)
        {


            // Iterate through reactions, if the reaction matches the ability then find the reaction user's modifier and set it.
            foreach (var triggeredReaction in triggeredReactions)
            {
                foreach (var effect in ability.EffectPowerDict.Keys)
                {
                    if (triggeredReaction.GetReaction().IsMatch(effect))
                    {
                        var reactionModifier = triggeredReaction.GetReactionModifiers();
                        
                        // Check if the modifier is lower than the best negative or higher than the best positive.
                        if (effectModifiersSelf[triggeredReaction.GetUser()].ContainsKey(effect)) // if the user of the reaction is already in the dictionary
                        {
                            if (effectModifiersSelf[triggeredReaction.GetUser()][effect].BestNegativeReactionModifier > reactionModifier)
                                effectModifiersSelf[triggeredReaction.GetUser()][effect].BestNegativeReactionModifier = reactionModifier;
                            else if (effectModifiersSelf[triggeredReaction.GetUser()][effect].BestPositiveReactionModifier < reactionModifier)
                                effectModifiersSelf[triggeredReaction.GetUser()][effect].BestPositiveReactionModifier = reactionModifier;
                        }
                        else
                        {
                            var power = new AbilityPower();
                            
                            // Create the AbilityPower data, and if the reaction has a negative or positive modifier then set that.
                            
                            if (reactionModifier > 0)
                            {
                                power.BestPositiveReactionModifier = reactionModifier;
                                effectModifiersSelf[triggeredReaction.GetUser()]
                                    .Add(effect, power);
                            }
                            else if (reactionModifier < 0)
                            {
                                power.BestNegativeReactionModifier = reactionModifier;
                                effectModifiersSelf[triggeredReaction.GetUser()]
                                    .Add(effect, power);
                            }
                            else
                            {
                                effectModifiersSelf[triggeredReaction.GetUser()]
                                    .Add(effect, power);
                            }
                        }
                    }

                }
            }

            return effectModifiersSelf;
        }

        public static Dictionary<Battler, Dictionary<EffectV2, AbilityPower>> GetResistancesOrVulnerabilities(
            AbilityV2 ability, Dictionary<Battler, Dictionary<EffectV2, AbilityPower>> effectModifiersSelf)
        {

            foreach (var battlerAndEffect in effectModifiersSelf)
            {
                foreach (var effectAndAbilityPower in battlerAndEffect.Value)
                {
                    effectAndAbilityPower.Value.ResistanceVulnerability =
                        battlerAndEffect.Key.BattlerStats.GetResistanceOrVulnerability(effectAndAbilityPower.Key, effectAndAbilityPower.Value.ResistanceVulnerability);
                }
            }
            return effectModifiersSelf;
        }

        public static Dictionary<Battler, Dictionary<EffectV2, AbilityPower>> CalculateHitOrMiss(
            AbilityV2 ability, Dictionary<Battler, Dictionary<EffectV2, AbilityPower>> effectModifiersSelf)
        {
            foreach (var kv in effectModifiersSelf)
            {
                var offensiveEffects = kv.Value.Where(kv => kv.Key.targettingType is EffectV2.ETargettingType.PrimaryTarget
                    or EffectV2.ETargettingType.AllExcludeUser or EffectV2.ETargettingType.AllExcludeUserAndPrimary);
                foreach (var offensiveEffect in offensiveEffects)
                    offensiveEffect.Value.HitsTarget = true;//todo: needs real opposed roll formula for hit/miss
            }
            return effectModifiersSelf;
        }

        public class AbilityPower
        {
            public float BestNegativeReactionModifier { get; set; } // Subtractive modifier to UserPower. 
            public float BestPositiveReactionModifier { get; set; } // Additive modifier to UserPower.
            public float ReactionModifier => BestNegativeReactionModifier + BestPositiveReactionModifier;
            public float TotalModifier => ReactionModifier + ResistanceVulnerability;
            public float ResistanceVulnerability { get; set; }
            public float UserPower { get; set; } = 1.0f;
            public bool HitsTarget { get; set; }
            public bool IsCriticalHit { get; set; }

            public float ApplyPowerToValue(float value)
            {
                value = value * UserPower;
                value = value * (1 + (BestNegativeReactionModifier + BestPositiveReactionModifier));
                value = value * ResistanceVulnerability;
                value = Mathf.Max(value, 0);
                return value;
            }

            public float GetFinalPower()
            {
                var power = UserPower + TotalModifier;
                power = Mathf.Max(power, 0);
                return power;
            }

            public AbilityPower(float p) => UserPower = p;

            public AbilityPower(){}
        }

        public static Dictionary<Battler, bool> CalculateHitOrMiss(AbilityV2 ability, List<Battler> targets)
        {
            var hitsResults =  new Dictionary<Battler, bool>(); // True: hit. False: miss.
            foreach (var target in targets)
            {
                hitsResults.Add(target, true);
            }

            return hitsResults;
        }

        public static void ApplyEffects(AbilityV2 ability, Battler user, Battler target, Dictionary<Battler, Dictionary<EffectV2, AbilityPower>> abilityPowerDict)
        {
            foreach (var effect in ability.EffectPowerDict.Keys)
            {
                var found = abilityPowerDict[target].TryGetValue(effect, out var abilityPower);
                if (found)
                {
                    effect.DoEffect(target, CalculateEffectFinalValue(abilityPower, effect, ability));
                }
            }
        }

        public static Dictionary<Battler, BattleTextData> GetTargetsCombatText(AbilityV2 ability,  Dictionary<Battler, bool> targetsHitResults, Dictionary<Battler, Dictionary<EffectV2, AbilityPower>> effectModifiersFinal)
        {
            var targetsHitsText = new Dictionary<Battler, BattleTextData>(targetsHitResults.Count); 
            
            foreach (var target in targetsHitResults)
            {
                var textData = new BattleTextData();
                if (target.Value is false)
                {
                    textData.Results.Add(BattleTextData.EResults.Miss);
                    targetsHitsText.Add(target.Key, textData);
                }
                else // If target is hit
                {
                    foreach (var effectsAbilityPowers in effectModifiersFinal)
                    {
                        // Handle Immediate Health Damage (DecreaseStatV2 with ChangedStat set to HealthV2
                        var decreaseHealthAbilityPowers = effectsAbilityPowers.Value.Where(pair => (pair.Key is DecreaseStatImmediateV2 decrease && 
                            decrease.ChangedStat == StatV2.EStatType.Health)); // Find effects that decrease health
                        int healthDamage = 0;
                        foreach (var damageEffect in decreaseHealthAbilityPowers)
                        {
                            healthDamage += (int)(damageEffect.Value.GetFinalPower() * ability.EffectPowerDict[damageEffect.Key]); // Add the value of the effect in the ability multiplied by the effect's final Power.

                            switch (damageEffect.Value.ResistanceVulnerability)
                            {
                                case > 1.0f:
                                    textData.ResistancesVulnerability.Add((BattleTextData.EResults.TargetWeak,
                                        (damageEffect.Key as DecreaseStatImmediateV2).eDamageType));
                                    break;
                                case <= 0f:
                                    textData.ResistancesVulnerability.Add((BattleTextData.EResults.TargetImmune,
                                        (damageEffect.Key as DecreaseStatImmediateV2).eDamageType));
                                    break;
                                case < 1.0f:
                                    textData.ResistancesVulnerability.Add((BattleTextData.EResults.TargetResistant,
                                        (damageEffect.Key as DecreaseStatImmediateV2).eDamageType));
                                    break;
                            }
                        }
                        textData.Damage = healthDamage;
                        
                        // Handle Immediate Health Heal (IncreaseStatImmediateV2 set to HealthV2)
                        var increaseHealthEffects = effectsAbilityPowers.Value.Where(pair => (pair.Key is IncreaseStatImmediateV2 increase && 
                            increase.ChangedStat == StatV2.EStatType.Health)); // Find effects that increase health
                        int healthHeal = 0;
                        foreach (var healEffect in increaseHealthEffects)
                        {
                            healthHeal += (int)(healEffect.Value.GetFinalPower() * ability.EffectPowerDict[healEffect.Key]); // Add the value of the effect in the ability by the effect's final Power.
                        }
                        textData.Heal = healthHeal;
                    }
                    targetsHitsText.Add(target.Key, textData);
                }
            }
            return targetsHitsText;
        }

        // Formula: (Final Power) * (Value assigned to the effect in the Ability)
        public static float CalculateEffectFinalValue(AbilityPower abilityPower, EffectV2 effect, AbilityV2 ability)
        {
            return (abilityPower.GetFinalPower() * ability.EffectPowerDict[effect]); 
        }

        public class BattleTextData
        {
            public int Damage { get; set; } 
            public int Heal { get; set; }
            public List<EResults> Results { get; set; } = new List<EResults>();
            public List<string> CustomResults { get; set; } = new List<string>();

            public HashSet<(BattleTextData.EResults, DecreaseStatImmediateV2.EDamageType)> ResistancesVulnerability { get; set; } =
                new HashSet<(BattleTextData.EResults, DecreaseStatImmediateV2.EDamageType)>();

            public bool IsMiss => Results.Contains(EResults.Miss);

            private static Dictionary<EResults, string> ResultsTexts { get; set; } =
                new Dictionary<EResults, string>()
                {
                    { EResults.TargetImmune, "Immune" },
                    { EResults.TargetResistant, "Resistant" },
                    { EResults.TargetWeak, "Weakness" },
                    { EResults.Immune, "Immune" },
                    { EResults.CriticalHit, "Critical Hit" },
                    { EResults.Miss, "Miss" },
                    { EResults.Dodge, "Dodge" },
                    { EResults.Block, "Block" },
                };

            public enum EResults
            {
                TargetImmune,
                TargetResistant,
                TargetWeak,
                Immune,
                CriticalHit,
                Miss,
                Dodge,
                Block,
            }
        }
    }

 
}