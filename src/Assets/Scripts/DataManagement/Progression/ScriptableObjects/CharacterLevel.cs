using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu]
public class CharacterLevel : ScriptableObject
{
    [SerializeField]IAdditiveStats.StatModifiers statsBaseGain;
    [SerializeField] List<AbilityV2> abilities;
    [SerializeField] List<ReactionV2> reactions;
    [SerializeField] CharacterLevel nextLevel;
}