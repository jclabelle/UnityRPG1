using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RotaryHeart.Lib.SerializableDictionaryPro;
using UnityEngine;

namespace BattleV3.Animations
{
[CreateAssetMenu(menuName = "BattleV3/BattleAnimationsDB")]
    public class BattleAnimationsDB : ScriptableObject
    {
        [SerializeField] private AnimsDB animsDB = new AnimsDB()
        {
            { EType.Attack , null},
            { EType.Move , null},
            { EType.Idle , null},
            { EType.Cast , null}

        };
        public BattleAnimation Attack => animsDB[EType.Attack];
        public BattleAnimation Move => animsDB[EType.Move];
        public BattleAnimation Idle => animsDB[EType.Idle];
        
        [Serializable]
        public enum EType
        {
            Attack,
            Move,
            Idle,
            Cast,
        }

        public bool Contains(EType type, BattleAnimation.ELookDirection lookDirection)
        {
            if (animsDB.ContainsKey(type) is false)
                return false;

            if (animsDB[type] is null)
                return false;

            return animsDB[type].Contains(lookDirection);
        }

        public AnimationClip Get(EType type, BattleAnimation.ELookDirection lookDirection)
        {
            if (Contains(type, lookDirection) is false)
                return null;

            return animsDB[type].animations[lookDirection];
        }
        
        
        [System.Serializable]
        public class AnimsDB : SerializableDictionary<EType, BattleAnimation> {}
        
        [System.Serializable]
        public class BattleAnimationsStorage
        {
            public BattleAnimation[] battleAnimations;
        }

        public class ClipData
        {
            public EType Type { get; set; }
            public BattleAnimation.ELookDirection LookDirection { get; set; }
            public AnimationClip Clip { get; set; }
        }
    }
    
    
    



}