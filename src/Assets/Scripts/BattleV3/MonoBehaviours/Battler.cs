using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BattleV3
{
    [RequireComponent(typeof(SpriteRenderer), typeof(Animator), typeof(BattlerStatesManager))]
    public class Battler: MonoBehaviour, IDisplayableInformation, IBattlerReactions, IBattlerMaterial
    {
        private Initiative initiative;
        private BattlerData data;
        private IBattleUI battleUI;
        public IBattlerAbilities BattlerAbilities { get; private set; }
        public IBattlerReactions BattlerReactions { get; private set; }
        public IBattlerStats BattlerStats { get; private set; }
        
        public IBattlerGraphicsData BattlerGraphicsData { get; private set; }
        public IBattlerMaterial BattlerMaterial { get; private set; }
        public Animations.IBattlerAnimations Animations { get; private set; }
        
        public IBattlerMovement Movement { get; private set; }
        public IBattlerStates States { get; private set; }

        private ReactionV2 currentReaction = null;
        

        private void Awake()
        {
            initiative = new Initiative(9999);
        }

        private void Start()
        {
            battleUI = FindObjectOfType<BattleUI>();
            Animations = GetComponent<Animations.BattlerAnimationsController>();
            States = GetComponent<BattlerStatesManager>();
            if (GetComponent<BattlePlayerController>() is BattlePlayerController controller)
                Movement = controller;
        }

        private void Update()
        {
            // Debug.Log($"{this.GetDisplayableName()}'s init: {initiative.value} / {initiative.maxValue}");
            if (initiative.Elapsed)
            {
                if (Battle.BattleLock.TryGrabLock())
                {
                    initiative.Advance();
                    StartCoroutine((DoAction()));
                }
            }
            
            if(Battle.BattleClock.IsClockRunning)
                initiative.Advance();
        }

        private IEnumerator DoAction()
        {
            Debug.Log($"{GetDisplayableName()}: Started DoAction");
            AbilityV2 ability;
            var targets = new List<Battler>();
            Battler primaryTarget = null;
            
            if (gameObject.CompareTag("Player"))
            {
                battleUI.ShowBattleSelectActionMenu();
                
                while (true)
                {
                    (targets, ability) = battleUI.TryGetPlayerAction();
                    if (ability is null)
                        yield return null;
                    else
                    {
                        if ((targets is not null) && targets.Count > 0 && ability.HasEffectOfTargetType(EffectV2.ETargettingType.PrimaryTarget))
                            primaryTarget = targets[0]; // The first battler in targets is the primary target if the ability has an effect of targetting type PrimaryTarget
                        
 
                        break;
                    }
                }
            }
            else
            {
                ability = BattlerAbilities.GetAbility();
                targets.Add((Battle.GetPlayer()));
            }
            
            Debug.Log($"{GetDisplayableName()}: {ability.GetDisplayableName()}");

            
            // dictionary to ensure targets only trigger one reaction - time stamp is used to resolve reaction quality
            var timeStampsOfReactingTargets = new Dictionary<Battler, float>(); 
            
            // If this flips true, the ability is interrupted and will never instantiate;
            bool successfulInterrupt = false;

            if (ability.HasReactionWindow)
            {
                Battle.OpenReactionWindow(ability);
                battleUI.ShowActionNamePopup(gameObject.transform.position, ability.GetDisplayableName());
                
                var npcChancesToReact = Battle.ReactionWindow.Duration; // NPCs get one chance per second of Window Diration to trigger a reaction
                
                while (Battle.ReactionWindow.Elapsed is false && successfulInterrupt is false)
                {
                    Battle.ReactionWindow.Advance();

                    // Do NPC reactions if the ability user is the player && a second has passed since the last chance to react
                    if (Battle.IsPlayer(this) && 
                        npcChancesToReact > 0 && 
                        npcChancesToReact > Battle.ReactionWindow.RemainingTime)
                    {
                        npcChancesToReact--; // Decrement by one second
                        Debug.Log("Checking for NPC reactions!");
                        
                        foreach (var target in targets)
                        {
                            // If the NPC is not in the list of reacting target, and is successful in triggering a reaction
                            if (!timeStampsOfReactingTargets.ContainsKey(target) &&
                                BattleRules.LetTargetAttemptToReact(this, target, ability))
                            {
                                // The a reaction, check if the reaction is a match with the ability, and if so add them to the list on reacting targets
                                BattleAI.SetReaction(target, ability);
                                if(target.GetCurrentReaction().IsMatch(ability))
                                    timeStampsOfReactingTargets.Add(target, Time.time);
                                
                                // Check if the reaction is a successful interrupt, and if so stop the loop
                                successfulInterrupt = CheckForInterrupt(target, ability);
                                if (successfulInterrupt)
                                    break;
                            }
                        }
                    }
                    
                    // Try to see if the player triggered a reaction if the ability user is an AI
                    foreach (var target in targets.Where(target => Battle.IsPlayer(target) && !timeStampsOfReactingTargets.ContainsKey(target)))
                    {
                        SetCurrentReaction(battleUI.TryGetPlayerReaction());
                        if(target.HasCurrentReaction())
                            successfulInterrupt = CheckForInterrupt(target, ability);
                        if (successfulInterrupt)
                            break;
                    }
                    yield return null;
                }
            }

            string targetListText = String.Empty;;
            foreach (var target in targets)
            {
                targetListText += target.GetDisplayableName() + " ";
            }
            Debug.Log($"{GetDisplayableName()}: targets:{targetListText}");
            

            if (successfulInterrupt is false)
            {
                var triggeredReactions = InstantiateReactions(timeStampsOfReactingTargets, ability);
                GameObject triggeredAbilityObject = new GameObject(ability.GetDisplayableName(), typeof(MonoAbility));
                var triggeredAbility = triggeredAbilityObject.GetComponent<MonoAbility>();
                triggeredAbility.Trigger(ability, this, targets, triggeredReactions);
            
                while(triggeredAbility.IsDone is false)
                    yield return null;
                
                triggeredAbility.CallDestroy();
            }
            
            battleUI.HideActionNamePopup();
            
            Battle.BattleLock.ReleaseLock();
            
            Battle.DoEndOfActionCleanup();
            
            Debug.Log($"{GetDisplayableName()}: Finished DoAction");

        }
        
        

        // Return true on a successful interrupt
        private bool CheckForInterrupt(Battler target, AbilityV2 ability)
        {
            if (!target.GetCurrentReaction().IsInterrupt ||
                !BattleRules.ResolveInterruptProbability(target, target.BattlerReactions.GetCurrentReaction()))
                return false;
            
            Debug.Log($"{ability.GetDisplayableName()} was interrupted by {target.GetDisplayableName()} with {target.BattlerReactions.GetCurrentReaction().GetDisplayableName()}");
            return true;

        }

        public void SetValues(BattlerData battlerData)
        {
            data = battlerData;
            BattlerAbilities = data;
            BattlerGraphicsData = data;
            data.SpriteRenderer = GetComponent<SpriteRenderer>();
            BattlerStats = new BattlerStats(data);
            BattlerReactions = this;
            BattlerMaterial = this;
            initiative = new Initiative(data.InitiativeMaxValue);
        }

        public void SetCurrentReaction(AbilityV2 ability)
        {
            if (!gameObject.CompareTag("Player"))
            {
                BattlerReactions.SetCurrentReaction(BattlerAbilities.GetReactionToAbility(ability));;
            }
        }

        public List<IMonoReactions> InstantiateReactions(Dictionary<Battler, float> targetsTimeStamps, AbilityV2 ability)
        {
            var triggeredReactions = new List<IMonoReactions>(targetsTimeStamps.Count);
                
                foreach (var target in targetsTimeStamps)
                {
                    if (target.Key.BattlerReactions.HasCurrentReaction() && 
                        target.Key.BattlerReactions.IsCurrentReactionMatchForAbility(ability) &&
                        !target.Key.BattlerReactions.IsCurrentReactionInterrupt())
                    {
                        GameObject triggeredReactionObject = new GameObject(target.Key.BattlerReactions.GetCurrentReaction().GetDisplayableName(), typeof(MonoReaction));
                        var triggeredReaction = triggeredReactionObject.GetComponent<MonoReaction>();
                        triggeredReaction.SetValues(target.Key.BattlerReactions.GetCurrentReaction(), ReactionV2.GetReactionQuality(target.Value, ability, ability.BaseReactionWindowDuration), target.Key);
                        triggeredReactions.Add(triggeredReaction);
                    }
                }
                return triggeredReactions;
        }


        public string GetDisplayableName()
        {
            return data.ToString().Remove(
                data.ToString().Count() - (data.GetType().ToString().Count() + 3)
            );
        }

        public string GetDisplayableStats()
        {
            throw new NotImplementedException();
        }

        public string GetDisplayableDescription()
        {
            throw new NotImplementedException();
        }

        public Texture2D GetDisplayableIcon()
        {
            throw new NotImplementedException();
        }
        
        public void SetCurrentReaction(ReactionV2 reaction)
        {
            currentReaction = reaction;
        }

        public void ClearCurrentReaction()
        {
            currentReaction = null;
        }

        public bool HasCurrentReaction()
        {
            return (currentReaction);
        }

        public bool IsCurrentReactionInterrupt()
        {
            return currentReaction.types.Contains(ReactionV2.Type.Interrupt);
        }

        public ReactionV2 GetCurrentReaction()
        {
            return currentReaction;
        }
        
        public bool IsCurrentReactionMatchForAbility(AbilityV2 ability)
        {
            return currentReaction.IsMatch((ability));
        }


        public void SetMaterial(Material material)
        {
            GetComponent<SpriteRenderer>().material = material;
        }

        public void ResetMaterial()
        {
            SetMaterial(BattleFX.BattlerDefaultMaterial);
        }

        public void CallDestroy()
        {
            Destroy((gameObject));
        }
        
        public void CallDeactivate()
        {
            gameObject.SetActive(false);
        }
    }

    public interface IBattlerReactions
    {
        public bool IsCurrentReactionMatchForAbility(AbilityV2 ability);
        public void SetCurrentReaction(ReactionV2 reaction);
        public void ClearCurrentReaction();
        public bool HasCurrentReaction();
        public bool IsCurrentReactionInterrupt();
        public ReactionV2 GetCurrentReaction();
    }

    public interface IBattlerMaterial
    {
        public void SetMaterial(Material material);
        public void ResetMaterial();
    }
    
}