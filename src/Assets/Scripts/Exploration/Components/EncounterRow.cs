using BattleV3;
using UnityEngine;

namespace Exploration
{
    [System.Serializable]

    public class EncounterRow
    {
        [SerializeField] private BattleEncounter encounter;
        [SerializeField] private double weight;

        public BattleEncounter Encounter
        {
            get => encounter;
            set => encounter = value;
        }

        public double Weight
        {
            get => weight;
            set => weight = value;
        }
    }
}