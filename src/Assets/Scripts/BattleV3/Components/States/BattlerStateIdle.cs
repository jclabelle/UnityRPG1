using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BattleV3
{
    public class BattlerStateIdle : BattlerBaseState
    {

        public override bool CanEnterState => true;
        public override bool CanExitState => true;

        public override void OnEnterState()
        {
        }

        public override void OnExitState()
        {
        }
    }
}