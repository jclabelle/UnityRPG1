using System.Collections;
using System.Collections.Generic;
using BattleV3;
using UnityEngine;

// This is a immediate, direct change to a stat
[System.Serializable]
[CreateAssetMenu(menuName = "Battle/DecreaseStatV2")]
public class DecreaseStatImmediateV2 : ChangeStatImmediateV2
{
    public enum EDamageType
    {
        Blunt,
        Slash,
        Pierce,
        Fire,
        Ice,
        Lightning,
    }

    public EDamageType eDamageType;

    public override void DoEffect(BattleV3.Battler target, float value)
    {
        target.BattlerStats.DecreaseStatBy(ChangedStat, (int)value);
    }
    
    public override void DoEffect(BattleV3.Battler target, int value)
    {
        target.BattlerStats.DecreaseStatBy(ChangedStat, value);
    }

    public override void Print()
    {
        base.Print();
    }
}