using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Linq;

[CreateAssetMenu]
public class CharacterProgression : ScriptableObject
{
    // Character levels start at index 0. IE Character Level 1 = index 0

    // Stat gains for each levels.
    public int[] health;
    public int[] stamina;
    public int[] physicalAttack;
    public int[] physicalDefense;
    public int[] physicalSpeed;
    public int[] mana;
    public int[] magicalAttack;
    public int[] magicalDefense;
    public int[] magicalSpeed;

    // Three lists of Abilities gained, so that the player can gain up to 3 abilities per level up.
    // Character level is: Index + 1. IE Index [2] of firstAbility is the first ability gained when the character gets to level 3.
    // Index [2] of secondAbility is the second ability gained when the character gets to level 3.
    // If the ability is "NothingAtThisCharacterProgressionLevel", the character gains nothing.
    public AbilityV2[] firstAbility; 
    public AbilityV2[] secondAbility;
    public AbilityV2[] thirdAbility;

    // Three lists of Reactions gained, so that the player can gain up to 3 reactions per level up.
    public ReactionV2[] firstReaction;
    public ReactionV2[] secondReaction;
    public ReactionV2[] thirdReaction;

    int baseNum = 5;
    int incr = 2;
    int maxCharLevel = 3;

    

    public (int, int, int, int, int, int, int, int, int, List<AbilityV2>, List<ReactionV2>) GetMaxBattleData()
    {
        List<AbilityV2> allAbilities = new List<AbilityV2>();
        List<ReactionV2> allReactions = new List<ReactionV2>();

        AddAbilities(firstAbility, allAbilities);
        AddAbilities(secondAbility, allAbilities);
        AddAbilities(thirdAbility, allAbilities);

        AddReactions(firstReaction, allReactions);
        AddReactions(secondReaction, allReactions);
        AddReactions(thirdReaction, allReactions);


        return (health.Sum(), stamina.Sum(), mana.Sum(), physicalAttack.Sum(), physicalDefense.Sum(), physicalSpeed.Sum(),
            magicalAttack.Sum(), magicalDefense.Sum(), magicalSpeed.Sum(), allAbilities, allReactions);
    }

    void AddAbilities(AbilityV2[] abilitiesSource, List<AbilityV2> abilitiesDestination)
    {
        foreach (AbilityV2 a in abilitiesSource)
        {
            if (a.name != "NoNewAbilityAtThisLevel")
            {
                abilitiesDestination.Add(a);
            }
        }
    }

    void AddReactions(ReactionV2[] reactionsSource, List<ReactionV2> reactionsDestination)
    {
        foreach (ReactionV2 r in reactionsSource)
        {
            if (r.name != "NoNewReactionAtThisLevel")
            {
                reactionsDestination.Add(r);
            }
        }
    }

#if UNITY_EDITOR

    void Reset()
    {
        int[] statDefaults = MakeDefaults(baseNum, incr, maxCharLevel);
        physicalAttack = statDefaults;
        physicalDefense = statDefaults;
        physicalSpeed = statDefaults;
        magicalAttack = statDefaults;
        magicalDefense = statDefaults;
        magicalSpeed = statDefaults;

        int[] ressourceDefaults = MakeDefaults(baseNum * 5, incr * 5, maxCharLevel);
        health = ressourceDefaults;
        stamina = ressourceDefaults;
        mana = ressourceDefaults;

        AbilityV2[] abilities = MakeDefaultAbilities(maxCharLevel);
        firstAbility = abilities;
        secondAbility = abilities;
        thirdAbility = abilities;

        ReactionV2[] reactions = MakeDefaultReactions(maxCharLevel);
        firstReaction = reactions;
        secondReaction = reactions;
        thirdReaction = reactions;

    }

    int[] MakeDefaults(int _baseNum, int _incr, int _maxCharLevel)
    {
        int[] defaults = new int[maxCharLevel];

        for (int i = 0; i < defaults.Length; i++)
        {
            defaults[i] = _baseNum + (_incr * i);
        }

        return defaults;
    }

    AbilityV2[] MakeDefaultAbilities(int _maxCharLevel)
    {
        AbilityV2[] abilities = new AbilityV2[_maxCharLevel];

        for (int i = 0; i < abilities.Length; i++)
        {
            var guid = AssetDatabase.FindAssets("NoNewAbilityAtThisLevel");
            var path = AssetDatabase.GUIDToAssetPath(guid[0]);
            abilities[i] = (AbilityV2)AssetDatabase.LoadAssetAtPath(path, typeof(AbilityV2));
            abilities[i].name = "NoNewAbilityAtThisLevel";
        }

        return abilities;
    }

    ReactionV2[] MakeDefaultReactions(int _maxCharLevel)
    {
        ReactionV2[] reactions = new ReactionV2[_maxCharLevel];

        for (int i = 0; i < reactions.Length; i++)
        {
            var guid = AssetDatabase.FindAssets("NoNewReactionAtThisLevel");
            var path = AssetDatabase.GUIDToAssetPath(guid[0]);
            reactions[i] = (ReactionV2)AssetDatabase.LoadAssetAtPath(path, typeof(AbilityV2));
            reactions[i].name = "NoNewReactionAtThisLevel";
        }

        return reactions;
    }

    

#endif

}
