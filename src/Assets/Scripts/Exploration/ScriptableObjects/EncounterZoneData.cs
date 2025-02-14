using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Exploration
{
    public class EncounterZoneData : ScriptableObject
    {
        [field: SerializeField] private Sprite BattleBackground { get; set; }
        [field: SerializeField] private int EncounterChancePerUnit { get; set; }
        [SerializeField] private EncounterTable randomEncounters;
    }
}