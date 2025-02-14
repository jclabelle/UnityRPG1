using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Animancer;
using BattleV3;
using UnityEngine;

namespace BattleV3.Animations
{
   
   [RequireComponent(typeof(Animator), typeof(Animancer.AnimancerComponent))]
   public class BattlerAnimationsController : MonoBehaviour, IBattlerAnimations
   {
      [field: SerializeField] private BattleAnimationsDB battleAnimations;
      private AnimancerComponent Animancer { get; set; }
      private Animator Animator { get; set; }
      private List<AnimationClip> Queue { get; set; }
      public Vector2 LookDirection { get; set; }
      public BattleSfxSequence CurrentSequence { get; set; }


      public bool IsIdle { get; }

      public void LookAt(Vector2 cardinalVector)
      {
         LookDirection = BattleConverters.ResolveLookDirection(transform.position, cardinalVector);
      }

      public void LookAt(Transform other)
      {
         LookAt(other.position);
      }

      public void Start()
      {
         Animator = GetComponent<Animator>();
         Animancer = GetComponent<AnimancerComponent>();      }

      public void PlayAnimation(AnimationClip clip)
      {
         Animancer.Play(clip);
      }

      public void PlayAnimation(BattlerBaseState battlerState)
      {
         throw new System.NotImplementedException();
      }

      public bool PlayAnimation(BattleAnimationsDB.EType animationType, BattleAnimation.ELookDirection lookDirection)
      {
         if (battleAnimations.Contains(animationType, lookDirection) is false)
            return false;
         
         Animancer.Play(battleAnimations.Get(animationType, lookDirection));
         return true;
      }

      public void QueueAnimation(AnimationClip clip)
      {
         Queue.Add(clip);
      }

      public bool QueueAnimation(BattleAnimationsDB.EType animationType, BattleAnimation.ELookDirection lookDirection)
      {
         if (battleAnimations.Contains(animationType, lookDirection) is false)
            return false;
         
         Queue.Add(battleAnimations.Get(animationType, lookDirection));
         return true;
      }

      public bool QueueAnimation(BattlerBaseState battlerState)
      {
         throw new NotImplementedException();
      }

   }

   public interface IBattlerAnimations
   {
      public bool IsIdle { get; }
      public void LookAt(Vector2 direction);
      public void LookAt(Transform other);
      public void PlayAnimation(AnimationClip clip);
      public void PlayAnimation(BattlerBaseState battlerState);
      public bool PlayAnimation(BattleAnimationsDB.EType animationType, BattleAnimation.ELookDirection lookDirection);
      
      public void QueueAnimation(AnimationClip clip);

      public bool QueueAnimation(BattleAnimationsDB.EType animationType, BattleAnimation.ELookDirection lookDirection);
      public bool QueueAnimation(BattlerBaseState battlerState);



   }
}