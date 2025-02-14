using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BattleV3
{
    public class Battle: MonoBehaviour
    {
        private static BattleLock _battleLock = new BattleLock();
        private static Initiative _reactionWindow;

        public static ILock BattleLock => _battleLock;
        public static IClock BattleClock => _battleLock;
        public static IReactionWindow ReactionWindow => _reactionWindow;

        public static void OpenReactionWindow(AbilityV2 ability)
        {
            _reactionWindow = new Initiative(ability.BaseReactionWindowDuration);
            var battlers = FindObjectsOfType<Battler>();
            // SetBattlersReactions(battlers, ability);

        }

        public static void SetBattlersReactions(Battler[] battlers, AbilityV2 ability)
        {
            foreach (var battler in battlers)
                battler.SetCurrentReaction(ability);
        }

        public static void DoEndOfActionCleanup()
        {
            var battlers = FindObjectsOfType<Battler>();

            //Handle death
            for(int i = battlers.Length-1; i >= 0; i--) 
            {
                if (battlers[i].BattlerStats.IsAlive is false)
                    battlers[i].CallDeactivate();
                else
                    battlers[i].ClearCurrentReaction();
            }
            SendAllBattlersHome();
            TryToEndBattle();
        }

        public static void TryToEndBattle()
        {
            var battlers = FindObjectsOfType<Battler>();

            if (battlers.Length == 1 && IsPlayerAlive() )
                DoWin();
            else if (IsPlayerAlive() is false)
                DoLose();
        }

        private static void DoLose()
        {
            BattleLock.ForceGrabLock();
            Debug.Log("Lost!");
        }

        private static void DoWin()
        {
            BattleLock.ForceGrabLock();
            Debug.Log("Won!");

            var DataMngr = FindObjectOfType<DataManager>();
            var player = GetPlayer();
            var encounter = (FindObjectOfType<BattleInitializer>() as IInitializeBattle).Encounter;
            
            DataMngr.Player.CurrentStats.Health = player.BattlerStats.Health.CurrentValue;
            DataMngr.Player.CurrentStats.Stamina = player.BattlerStats.Stamina.CurrentValue;
            DataMngr.Player.CurrentStats.Mana = player.BattlerStats.Mana.CurrentValue;
            DataMngr.Player.CurrentStats.ClampAll(DataMngr.Player.MaxStats);

            foreach(var reward in encounter.GetRewards())
             DataMngr.PlayerRewards.Add(reward);
            
                
        }

        public static List<Battler> GetBattlers()
        {
            return FindObjectsOfType<Battler>().ToList();
        }
        
        public static List<Battler> GetBattlersExceptPlayer()
        {
            return FindObjectsOfType<Battler>().Where(battler => !battler.gameObject.CompareTag("Player")).ToList();
        }
        

        public static IBattleUI GetBattleUI()
        {
            return FindObjectOfType<BattleUI>() as IBattleUI;
        }

        public static IBattleEffects GetBattleFX()
        {
            return FindObjectOfType<BattleFX>();
        }

        public static Battler GetPlayer()
        {
            return GameObject.FindWithTag("Player").GetComponent<Battler>();
        }
        
        public static bool IsPlayerAlive()
        {
            return GameObject.FindWithTag("Player");
        }

        public static bool IsPlayer(Battler battler)
        {
            return battler.CompareTag("Player");
        }

        public static Vector2 GetCenterPositionOfBattlefield()
        {
            if (GameObject.FindGameObjectWithTag("BattlefieldCenter") is GameObject center)
                return center.transform.position;

            return new Vector2(0, 0);
        }

        public static bool AreAllBattlersIdleState()
        {
            var battlers = GameObject.FindObjectsOfType<Battler>();
            foreach (var battler in battlers)
            {
                if (battler.States.StateMachine.CurrentState != battler.States.Idle)
                    return false;
            }

            return true;
        }

        public static void SendAllBattlersHome()
        {
            foreach (var battler in Battle.GetBattlers())
            {
                if ( (battler.Movement is not null) && (battler.Movement.SpawnPoint.Position - battler.Movement.CurrentPosition).SqrMagnitude() > 0)
                {
                    battler.States.SetMove(new Movement(battler.Movement.CurrentPosition, battler.Movement.SpawnPoint.Position, 1.0f));
                }
            }
        }
    }

    public interface IAIBattlers
    {
    }
}