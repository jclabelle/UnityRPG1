using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BattleV3
{
    public class MonoAbility : MonoBehaviour
    {
        public bool IsDone { get; private set; }
        private Dictionary<EffectV2, float> BaseEffectModifiers { get; set; }

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }
        
        public void Trigger(AbilityV2 ability, Battler user, List<Battler> targets, List<IMonoReactions> triggeredReactions)
        {
            StartCoroutine(DoAbility(ability, user, targets, triggeredReactions));
        }

        private IEnumerator DoAbility(AbilityV2 ability, Battler user, List<Battler> targets,
            List<IMonoReactions> triggeredReactions)
        {
            IsDone = false;

            var battleFXMono = FindObjectOfType<BattleFX>();
            var battleSfx = battleFXMono.BattleEffects;
            // IBattlerAnimations battlerAnimations = battleFXMono.BattlerAnimations;

            Battler primaryTarget = null;

            // Save a reference for the primary target of the ability
            if (ability.HasEffectOfTargetType(EffectV2.ETargettingType.PrimaryTarget))
                primaryTarget = targets[0];

            // Get the base power of the ability's effects according to it's users stats and capabilities
            var effectPower = BattleRules.GetEffectsBasePower(ability, user);
            
            // Modify the power according to reactions affecting all target's final power values
            var effectModifiersAllTargets =
                BattleRules.ApplyReactionsModifiersIncomingAll(ability, triggeredReactions, effectPower);

            // Match batters with the appropriate Effects according to EffectV2.EtargettingType
            var battlerAndEffectsDic =
                BattleRules.AssignEffectsToBattlers(effectModifiersAllTargets, user, primaryTarget);
            
            // Modify the power for each target according to self-only reactions
            var effectModifiersSelf =
                BattleRules.ApplyReactionsModifiersIncomingSelf(ability, battlerAndEffectsDic, triggeredReactions);
            
            // Modify the power according to each target's resistances or vulnerabilities
            var effectModifiersFinal = BattleRules.GetResistancesOrVulnerabilities(ability, effectModifiersSelf);
            
            // Check if the ability misses certain targets
            var targetsHitResults = BattleRules.CalculateHitOrMiss(ability, targets);
            var effectModifiersFinalHitMiss = BattleRules.CalculateHitOrMiss(ability, effectModifiersFinal);

            // Create combat text
            var targetsText = BattleRules.GetTargetsCombatText(ability, targetsHitResults, effectModifiersFinal);
            
            
            // Player must satisfy Ability Positioning Requirement
            if (Battle.GetPlayer() == user)
            {
                Vector2 movementDestination = Vector2.left;
                if (primaryTarget)
                    movementDestination = primaryTarget.transform.position;
                
                switch (ability.MovementType)
                {
                    case Movement.EType.MoveToTarget:
                        user.States.SetMove(new Movement(user.transform.position, movementDestination, Movement.EType.MoveToTarget, user.Movement.MoveSpeed, 1.0f, 1.0f));
                        break;
                    case Movement.EType.ShortStepTowardsTarget:
                        user.States.SetMove(new Movement(user.transform.position, movementDestination, Movement.EType.ShortStepTowardsTarget, user.Movement.MoveSpeed, 0f, 1.0f));
                        break;
                    case Movement.EType.MoveToCenterOfBattle:
                        user.States.SetMove(new Movement(user.transform.position, Battle.GetCenterPositionOfBattlefield(), Movement.EType.MoveToCenterOfBattle, user.Movement.MoveSpeed, 1.0f, 1.0f));
                        break;
                    case Movement.EType.StayInPlace:
                        user.States.SetMove(new Movement(user.transform.position, Battle.GetCenterPositionOfBattlefield(), Movement.EType.MoveToCenterOfBattle, user.Movement.MoveSpeed, 1.0f, 1.0f));
                        break;
                }
            }

            // Wait for the ability's user to get in position
            while (user.States.StateMachine.CurrentState == user.States.Move)
            {
                yield return null;
            }

            // Save the list of batters who got hit in a dictionary.
            var xx = targetsHitResults.Where(battler => battler.Value is true).ToDictionary(k => k.Key, v =>v.Value).Keys;

            // Save a list of all battlers: the user, the primary target, all hit battlers, all missed battlers
            var participatingBattlers = new ParticipatingBattlers(user, primaryTarget,
                targetsHitResults.Where(battler => battler.Value is true).ToDictionary(k => k.Key, v => v.Value).Keys
                    .ToList(),
                targetsHitResults.Where(battler => battler.Value is false).ToDictionary(k => k.Key, v => v.Value).Keys
                    .ToList());
            
            // Switch the ability user's state to Use Ability, and set the state of reacting battlers to Use Reaction
            user.States.SetUseAbility(ability);
            foreach (var triggeredReaction in triggeredReactions)
                triggeredReaction.GetUser().States.SetUseReaction(triggeredReaction.GetReaction());

            // Create the ability's VFX monobehaviour, and give it the list of participating battlers
            Animations.MonoSfxPlayer.Make(ability.SfxSequence, participatingBattlers);

            // Create the reaction's VFX monobehaviours
            foreach (var triggeredReaction in triggeredReactions)
                Animations.MonoSfxPlayer.Make(triggeredReaction.GetReaction().SfxSequence, Vector3.zero);
            
            // Wait for all battlers to be idle and all VFX monobehaviours to have played out.
            while (Battle.AreAllBattlersIdleState() is false || battleSfx.AreTriggeredSfxDone() is false)
            {
                yield return null;
            }
            
            
            // Apply all ability and reaction effects
            foreach (var target in targets)
            {
                if(targetsHitResults.ContainsKey(target))
                    BattleRules.ApplyEffects(ability, user, target, effectModifiersSelf);
            }
            
            // Clean up reactions
            foreach (var triggered in triggeredReactions)
            {
                triggered.CallDestroy();
            }
            
            // Debug stuff.
            foreach(var item in targetsText)
                Debug.Log($"{item.Key.GetDisplayableName()} takes {item.Value.Damage } dmg, has {item.Key.BattlerStats.Health.CurrentValue} health remaining");
            
            // Switch the MonoAbility's flag to done, so that the Battler can stop waiting and start cleaning up.
            IsDone = true;
        }

        private float FindBaseModifier()
        {
            return 0;
        }

        public void CallDestroy()
        {
            UnityEngine.Object.Destroy(gameObject);
        }

        public class ParticipatingBattlers
        {
            public List<Battler> AllHitTargets { get; set; }
            public List<Battler> AllMissedTargets { get; set; }
            public List<Battler> AllTargets
            {
                get
                {
                    var all = new List<Battler>();
                    all.AddRange(AllHitTargets);
                    all.AddRange(AllMissedTargets);
                    return all;
                }
            }

            public List<Battler> AllHitsExcludePrimary
            {
                get
                {
                    if (PrimaryTarget is null)
                        return AllHitTargets;
                    
                    var allExclPrimary = new List<Battler>();
                    allExclPrimary.AddRange(AllHitTargets.Where(target => target != PrimaryTarget));
                    return allExclPrimary;
                }
            }

            public List<Battler> AllMissExcludePrimary
            {
                get
                {
                    if (PrimaryTarget is null)
                        return AllMissedTargets;
                    
                    var allExclPrimary = new List<Battler>();
                    allExclPrimary.AddRange(AllMissedTargets.Where(target => target != PrimaryTarget));
                    return allExclPrimary;
                }
            }

            public Battler PrimaryTarget { get; set; }
            public Battler User { get; set; }

            public ParticipatingBattlers(Battler user, Battler primaryTarget, List<Battler> allHits,
                List<Battler> allMiss)
            {
                User = user;
                PrimaryTarget = primaryTarget;
                AllHitTargets = allHits;
                AllMissedTargets = allMiss;
            }
        }
    }


}