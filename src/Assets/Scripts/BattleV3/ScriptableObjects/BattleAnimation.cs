using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BattleV3;
using NaughtyAttributes;
using RotaryHeart.Lib.SerializableDictionaryPro;
using UnityEngine;

namespace BattleV3.Animations
{
    
    
    [CreateAssetMenu(menuName = "BattleAnimation")]
    public class BattleAnimation : ScriptableObject
    {
        [SerializeField] public AnimsDict animations = new AnimsDict()
        {
            { ELookDirection.Right, null },
            { ELookDirection.Left, null },
            { ELookDirection.Up, null },
            { ELookDirection.Down, null },
            { ELookDirection.RightOrLeft, null },
            { ELookDirection.Any, null },
        };

        [field: SerializeField] public SfxSequenceDict Sequences = new SfxSequenceDict()
        {
            { ELookDirection.Right, null },
            { ELookDirection.Left, null },
            { ELookDirection.Up, null },
            { ELookDirection.Down, null },
            { ELookDirection.RightOrLeft, null },
            { ELookDirection.Any, null },
        };

        public AnimationClip Right => animations[ELookDirection.Right];
        public AnimationClip Left => animations[ELookDirection.Left];
        public AnimationClip Up => animations[ELookDirection.Up];
        public AnimationClip Down => animations[ELookDirection.Down];
        public AnimationClip RightOrLeft => animations[ELookDirection.RightOrLeft];
        public AnimationClip Any => animations[ELookDirection.Any];


        [Serializable]
        public enum ELookDirection
        {
            Right,
            Left,
            Up,
            Down,
            RightOrLeft,
            Any,
        }

        public bool Contains(ELookDirection lookDirection)
        {
            if (animations.ContainsKey(lookDirection) is false)
                return false;

            if (animations[lookDirection] is null)
                return false;

            return true;
        }


        public static ELookDirection MovementToLook(IMovement.EDirection eDirection)
        {
            return eDirection switch
            {
                IMovement.EDirection.Right => ELookDirection.Right,
                IMovement.EDirection.Up => ELookDirection.Up,
                IMovement.EDirection.Left => ELookDirection.Left,
                IMovement.EDirection.Down => ELookDirection.Down,
                _ => throw new ArgumentOutOfRangeException(nameof(eDirection), eDirection, null)
            };
        }


        [Serializable]
        public class AnimsDict : SerializableDictionary<ELookDirection, AnimationClip>
        {
        }

        [Serializable]
        public class AnimationsArray
        {
            public AnimationClip[] clips;
        }

        [Serializable]
        public class SfxSequenceDict : SerializableDictionary<ELookDirection, BattleSfxSequence>
        {
        }

        [Serializable]
        public class SfxSequenceArray
        {
            public BattleSfxSequence[] items;
        }
    }
}