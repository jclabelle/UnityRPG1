using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public abstract class Equipment : Item, IAdditiveStats, IAddAbilities, IAddReactions, IEquippable
{
   

    [SerializeField] IAdditiveStats.StatModifiers stats;
    [SerializeField] AbilityV2[] abilities;
    [SerializeField] ReactionV2[] reactions;

    public IAdditiveStats.StatModifiers GetAdditiveModifiers()
    {
        return Stats;
    }

    public AbilityV2[] Abilities { get => abilities; }
    public ReactionV2[] Reactions { get => reactions; }
    public IAdditiveStats.StatModifiers Stats { get => stats; set => stats = value; }

    // todo: Add Abilities and Reactions
    public override Newtonsoft.Json.Linq.JObject GetSaveGameJson()
    {
        var JO = base.GetSaveGameJson();

        return JO;

    }

    public override string GetDisplayableStats()
    {
        return GetStatsAsString();
    }

    public override string GetDisplayableDescription()
    {
        return Description;
    }

    public override string GetStatsAsString()
    {
        var str =  base.GetStatsAsString();
        var mods = GetAdditiveModifiers();

        str += $"\n{IAdditiveStats.StatModifiers.GetStringColumns(mods)}";

        if (Abilities.Length > 0 || Reactions.Length > 0)
        {
            str += "Specials:\n";

            if(Abilities.Length > 0)
            {
                str += "\n\n Abilities\n";
                foreach (var ability in Abilities)
                    str += $"{ability.SaveGameName}, ";
            }

            if(Reactions.Length > 0)
            {
                str += "\n\n Reactions\n";
                foreach (var reaction in Reactions)
                    str += $"{reaction.SaveGameName}, ";
            }
        }

        return str;
    }
}

public interface IEquippable
{

}





