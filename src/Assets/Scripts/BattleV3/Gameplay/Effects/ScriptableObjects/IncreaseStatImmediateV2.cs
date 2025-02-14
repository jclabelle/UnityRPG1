using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is a immediate, direct change to a stat
[CreateAssetMenu(menuName = "Battle/IncreaseStatImmediateV2")]
public class IncreaseStatImmediateV2 : ChangeStatImmediateV2
{
    
    public override void DoEffect(BattleV3.Battler target, float value)
    {
        target.BattlerStats.IncreaseStatBy(ChangedStat, (int)value);
    }
    
    public override void DoEffect(BattleV3.Battler target, int value)
    {
        target.BattlerStats.IncreaseStatBy(ChangedStat, value);
    }

    public override void Print()
    {
        base.Print();
    }
}
