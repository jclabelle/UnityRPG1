using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BattleV3
{
    [CreateAssetMenu(menuName = "BattleV3/BattlerData")]

    public class BattlerData : ScriptableObject, IBattlerAbilities, IBattlerGraphicsData
    {
        [field: SerializeField] public Sprite Sprite{ get; set; }
        [field: SerializeField] public Vector3 Scale { get; set; }
        // x = -1 -> Appears to be facing left
        // x = 1 -> Appears to be facing right
        // y = 0 -> Neutral, could be positioned either left, above or right of the character
        // y = -1 -> Needs to be positioned somewhat above the character to make sense
        [field: SerializeField] public Vector2 SpriteDefaultFacing { get; set; }
        [field: SerializeField] public ESpriteFacing SpriteAppearsToLookTowards { get; set; }
        public void FlipSpriteX()
        {
            SpriteRenderer.flipX = true;
        }


        [field: SerializeField] public float InitiativeMaxValue { get; set; }
        [field:SerializeField] public IAdditiveStats.StatModifiers Stats { get; set; }
        [field: SerializeField] public List<AbilityV2> Abilities { get; set; }
        [field: SerializeField] public List<ReactionV2> Reactions { get; set; }
        [field: SerializeField] public Rewards.LootTable LootTable { get; set; }
        public SpriteRenderer SpriteRenderer { private get; set; }
      

        public IReward.Reward Reward { get; set; }

        public enum ESpriteFacing
        {
            Up,
            Down,
            Left,
            Right
        }

        public AbilityV2 GetAbility()
        {
            return Abilities[0];
        }

        public ReactionV2 GetReactionToAbility(AbilityV2 ability)
        {
            return Reactions[0];
        }

        public void SetValues(
            IAdditiveStats.StatModifiers stats,
            List<AbilityV2> abilities,
            List<ReactionV2> reactions)
        {
            Stats = stats;
            Abilities = abilities;
            Reactions = reactions;
        }

    }

    public interface IBattlerAbilities
    {
        public AbilityV2 GetAbility();
        public ReactionV2 GetReactionToAbility(AbilityV2 ability);
    }

    public interface IBattlerGraphicsData
    {
        public Sprite Sprite{ get; set; }
        public Vector2 SpriteDefaultFacing { get; set; }
        public Vector3 Scale { get; set; }
         public BattlerData.ESpriteFacing SpriteAppearsToLookTowards { get; set; }
        public void FlipSpriteX();

    }
    

}