using System;
using UnityEngine;
namespace Rewards
{
    [Serializable]
    public class LootRow
    {
        [SerializeField] private Item item;
        [SerializeField] private double weight;

        public Item Item
        {
            get => item;
            set => item = value;
        }

        public double Weight
        {
            get => weight;
            set => weight = value;
        }
    }
}