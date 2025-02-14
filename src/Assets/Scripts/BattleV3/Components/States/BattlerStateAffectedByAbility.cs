using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BattleV3;
using UnityEngine;

public class BattlerStateAffectedByAbility : BattlerBaseState
{
    public override bool CanEnterState { get; }
    public override bool CanExitState { get; }
    public override void OnEnterState()
    {
        throw new System.NotImplementedException();
    }

    public override void OnExitState()
    {
        throw new System.NotImplementedException();
    }
}