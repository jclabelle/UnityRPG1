using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Animancer.FSM;
using BattleV3.Animations;
using UnityEngine;

namespace BattleV3
{
    public class BattlerStatesManager : MonoBehaviour, IBattlerStates
    {
        public StateMachine<IState> StateMachine { get;  set; } = new StateMachine<IState>();
        public BattlerStateIdle Idle { get; set; } = new BattlerStateIdle();
        public BattlerStateMove Move { get; set; } = new BattlerStateMove();
        public BattlerStateUseAbility UseAbility { get; set; } = new BattlerStateUseAbility();
        public BattlerStateUseReaction UseReaction { get; set; } = new BattlerStateUseReaction();
        public BattlerAnimationsController AnimationsController { get; set; }


        public void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            var controller = GetComponent<BattlePlayerController>();
            AnimationsController = GetComponent<BattlerAnimationsController>();
            if (controller is not null)
                Move.BattlerMovement = controller;
            SetIdle();
        }
        
        public void Update()
        {
            if(StateMachine.CurrentState == Move && Move.CanExitState)
                SetIdle();
                
        }

        public void SetUseAbility(AbilityV2 ability)
        {
            StateMachine.TrySetState(UseAbility);
        }

        public void SetUseReaction(ReactionV2 reaction)
        {
            StateMachine.TrySetState(UseReaction);
        }

        public void SetIdle()
        {
            StateMachine.TrySetState(Idle);
            if(AnimationsController)
                AnimationsController.PlayAnimation((BattleAnimationsDB.EType.Idle), BattleAnimation.ELookDirection.Left);
        }

        public void SetMove(Movement movement)
        {
            Move.BattlerMovement.MoveTo(movement);
            var lookDirectionCardinal =
                BattleConverters.ResolveLookDirection(movement.LocalPositionOrigin,
                    movement.LocalPositionDestination);
            var eMovementDirection = BattleConverters.Vector2ToDirection(lookDirectionCardinal);
            var eLookDirection = BattleAnimation.MovementToLook(eMovementDirection);
            AnimationsController.PlayAnimation(BattleAnimationsDB.EType.Move, eLookDirection);
            StateMachine.TrySetState(Move);
        }

    }

    public interface IBattlerStates
    {
        public void SetUseAbility(AbilityV2 ability);
        public void SetUseReaction(ReactionV2 reaction);
        public void SetIdle();
        public void SetMove(Movement movement);
        public StateMachine<IState> StateMachine { get; set; }
        
        public BattlerStateIdle Idle { get; set; } 
        public BattlerStateMove Move { get; set; }
        public BattlerStateUseAbility UseAbility { get; set; } 
        public BattlerStateUseReaction UseReaction { get; set; } 

    }
}

