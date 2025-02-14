using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Battle/ReactStatDecreaseV2")]
public class ReactStatDecreaseV2 : ReactionV2
{
    public List<StatV2.EStatType> valueModifierStat;
    public List<StatV2.EStatType> changedStat;
    public List<DecreaseStatImmediateV2.EDamageType> dmgType;
    public float reactionModifierWeak;
    public float reactionModifierAverage;
    public float reactionModifierGood;
    public float reactionModifierGreat;
    public float reactionModifierPerfect;

    public override bool IsMatch(EffectV2 effect)
    {
        if (base.IsMatch(effect) == false)
        {
            return false;
        }

        DecreaseStatImmediateV2 effectCast = (DecreaseStatImmediateV2)effect;
        // Debug.Log("After cast: " + effectCast.BaseFormulaStat + " " + effectCast.ChangedStat);


        if (valueModifierStat.Contains(effectCast.BaseFormulaEStat) == false)
        {
            return false;
        }

        if (changedStat.Contains(effectCast.ChangedStat) == false)
        {
            return false;
        }

        if (dmgType.Contains(effectCast.eDamageType) == false)
        {
            return false;
        }

        return true;
    }

    public override float GetReactionModifier(ReactionQuality qual)
    {
        switch (qual)
        {
            case ReactionQuality.Weak:
                {
                    return reactionModifierWeak;
                }
            case ReactionQuality.Average:
                {
                    return reactionModifierAverage;
                }
            case ReactionQuality.Good:
                {
                    return reactionModifierGood;
                }
            case ReactionQuality.Great:
                {
                    return reactionModifierGreat;
                }
            case ReactionQuality.Perfect:
                {
                    return reactionModifierPerfect;
                }
        }
        return 0;
    }

}
