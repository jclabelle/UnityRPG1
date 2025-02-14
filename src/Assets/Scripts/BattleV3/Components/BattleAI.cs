using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BattleV3
{
    public class BattleAI: IAIBattlers
    {
        public static void SetReaction(Battler AI, AbilityV2 ability)
        {
            AI.BattlerReactions.SetCurrentReaction(AI.BattlerAbilities.GetReactionToAbility(ability));
        }
    }
}