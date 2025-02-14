using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BattleV3
{[CreateAssetMenu(menuName= "BattleV3/BattleEncounter")]
    public class BattleEncounter : ScriptableObject
    {
        [field: SerializeField] public List<BattlerData> Enemies { get; private set; }

        public List<IReward.Reward> GetRewards()
        {
            return Enemies.Select(enemy => enemy.Reward).ToList();
        }
    }
    
}