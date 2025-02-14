using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BattleV3
{
    public class BattlerStateUseAbility : BattlerBaseState
    {
        public override bool CanEnterState { get; }
        public override bool CanExitState { get; }

        public override void OnEnterState()
        {
            throw new NotImplementedException();
        }

        public override void OnExitState()
        {
            throw new NotImplementedException();
        }
    }
}