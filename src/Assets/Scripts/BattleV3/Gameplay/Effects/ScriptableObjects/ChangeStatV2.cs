using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeStatV2 : EffectV2
{



    private StatV2.EStatType baseFormulaEStat;
    

    public int EffectMultiplier { get ; set; }
    public StatV2.EStatType BaseFormulaEStat { get => baseFormulaEStat; set => baseFormulaEStat = value; }
    [field:SerializeField]public StatV2.EStatType ChangedStat { get ; set; }
    [field: SerializeField] public StatV2.EStatType PowerBasedOn { get; set; }
}
