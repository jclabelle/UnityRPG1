using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Rewards
{
    
    [System.Serializable]
    public class LootTable
    {
        [SerializeField] private List<LootRow> rows;
        [SerializeField] const int numberOfRolls = 1;

        public List<LootRow> Rows
        {
            get => rows;
            set => rows = value;
        }

        public int NumberOfRolls
        {
            get => numberOfRolls;
        }

        public List<Item> Roll(int rolls = numberOfRolls)
        {
            var loot = new List<Item>();

            if (Rows is List<LootRow> lootRows)
            {

                List<LootRow> sorted = Rows.OrderBy(row => row.Weight).ToList<LootRow>();

                while (rolls > 0)
                {
                    double rnd = Random.Range(0f, 1f);
                    double odds = 0;
                    rolls -= 1;

                    for (int i = 0; i < sorted.Count; i++)
                    {
                        odds += sorted[i].Weight;

                        Debug.Log(
                            $"Rolling for loot: odds for Item {sorted[i].Item.name} land between {odds - sorted[i].Weight} and {odds} while roll is {rnd}");

                        if (rnd <= odds)
                        {
                            loot.Add(sorted[i].Item);
                            Debug.Log($"Player won {sorted[i].Item}!");
                            break;
                        }
                    }
                }

            }

            return loot;
        }
    }
}