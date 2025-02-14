using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using System.Linq;


[CreateAssetMenu(menuName = "Equipment/Weapon"), System.Serializable]
public class Weapon : Equipment, IDamage, IHandHeld
{
    [SerializeField] int damageMax;
    [SerializeField] int damageMin;
    [SerializeField] DecreaseStatImmediateV2.EDamageType eDamageType;
    [SerializeField] StatV2.EStatType damagedEStat;
    [SerializeField] StatV2.EStatType valueBasedOn;
    [SerializeField] private bool isTwoHander;
    public DecreaseStatImmediateV2.EDamageType EDamageType { get => eDamageType; }
    public int DamageMax { get => damageMax; }
    public int DamageMin { get => damageMin; }
    public StatV2.EStatType ValueBasedOn { get => valueBasedOn; }
    public StatV2.EStatType DamagedEStat { get => damagedEStat; }
    public bool IsTwoHander { get => isTwoHander; set => isTwoHander = value; }

    public override Newtonsoft.Json.Linq.JObject GetSaveGameJson()
    {
        var JO = base.GetSaveGameJson();

        return JO;

    }

    public override string GetStatsAsString()
    {
        var str = base.GetStatsAsString();
        str += "\nDamage:\n";
        str += $"Damage Range: {DamageMin} - {DamageMax}\tType: {EDamageType.ToString()}";
        str += $"Affects: {DamagedEStat.ToString()}\tUses: {ValueBasedOn.ToString()}";
        str += $"Affects: {DamagedEStat.ToString()}\tUses: {ValueBasedOn.ToString()}";

        if(isTwoHander is true)
        {
            str +=$"\nRequires both hands to wield.";
        }

        return str;
    }


}

