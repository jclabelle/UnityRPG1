using System.Collections.Generic;
using System.Linq;
using BattleV3;
using UnityEngine;

namespace Exploration
{
    [System.Serializable]

    public class EncounterTable
    {
        [SerializeField] private List<EncounterRow> rows;
        [SerializeField] const int numberOfRolls = 1;

        public List<EncounterRow> Rows
        {
            get => rows;
            set => rows = value;
        }

        public int NumberOfRolls
        {
            get => numberOfRolls;
        }

        public bool WeightsLessOrEqualOne()
        {
            double sum = 0.0;
            foreach (var encounter in rows)
            {
                sum += encounter.Weight;
            }

            return sum <= 1.0;
        }

        public List<BattleEncounter> Roll(int rolls = numberOfRolls)
        {
            var loot = new List<BattleEncounter>();

            if (Rows is List<EncounterRow> lootRows)
            {

                List<EncounterRow> sorted = Rows.OrderBy(row => row.Weight).ToList<EncounterRow>();

                while (rolls > 0)
                {
                    double rnd = Random.Range(0f, 1f);
                    double odds = 0;
                    rolls -= 1;

                    for (int i = 0; i < sorted.Count; i++)
                    {
                        odds += sorted[i].Weight;

                        Debug.Log(
                            $"Rolling for Encounter: odds for Encounter {sorted[i].Encounter.name} land between {odds - sorted[i].Weight} and {odds} while roll is {rnd}");

                        if (rnd <= odds)
                        {
                            loot.Add(sorted[i].Encounter);
                            Debug.Log($"Player will battle: {sorted[i].Encounter}!");
                            break;
                        }
                    }
                }

            }

            return loot;
        }
    }
}