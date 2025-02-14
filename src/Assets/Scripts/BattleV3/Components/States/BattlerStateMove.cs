using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BattleV3.Animations;
using UnityEngine;

namespace BattleV3
{
    
    public class BattlerStateMove : BattlerBaseState
    {
        public IBattlerMovement BattlerMovement { get; set; }
        public IBattlerAnimations BattlerAnimations { get; set; }
        public override bool CanEnterState => BattlerMovement is not null;
        public override bool CanExitState => !BattlerMovement.IsMoving;

        public override void OnEnterState()
        {
                
        }

        public override void OnExitState()
        {
            
        }
    }
}