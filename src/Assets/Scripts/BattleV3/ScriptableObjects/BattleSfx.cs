using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Animancer;
using Animancer.Examples.Events;
using DG.Tweening;
using UnityEngine;
using NaughtyAttributes;

namespace BattleV3.Animations
{

        [CreateAssetMenu(menuName = "BattleSFX")]

        public class BattleSfx : ScriptableObject
        {

            [field: SerializeField, Required("Clip is required")] public AnimationClip Clip { get; set; }
            [field: SerializeField] public List<AudioEvent> AudioTimelines { get; set; }
            [field: SerializeField] public List<PositionEvent> Movements { get; set; }
            [field: SerializeField] public List<MaterialEvent> Materials { get; set; }

            public AnimancerState MakeAnimancerState(AnimancerComponent animancer, AudioSource audioPlayer,
                GameObject gObject, bool isInfinite, Battler battler = null, bool applyMaterials = true)
            {

                var state = animancer.Play(Clip);
                state.Time = 0;

                if (isInfinite) // force looping
                    state.Events.OnEnd = EventUtilities.RestartCurrentState;
                
                // Trigger times are normalized time based on animation length
                // An event that must play halfway through the animation will have a trigger time of 0.5
                foreach (var audioClip in AudioTimelines)
                {
                    if(audioClip.Clip)
                        state.Events.Add(audioClip.TriggerTime, () => audioPlayer.PlayOneShot(audioClip.Clip));
                }

                foreach (var movement in Movements)
                {
                    state.Events.Add(movement.TriggerTime,
                        () =>
                        {
                            if(movement.Movement.LocalPositionOrigin.sqrMagnitude > 0)
                                gObject.transform.DOMove(
                                    (gObject.transform.position + (Vector3)movement.Movement.LocalPositionOrigin),
                                    0).From();
                            
                            gObject.transform.DOMove(
                                (gObject.transform.position + (Vector3)movement.Movement.LocalPositionDestination),
                                movement.Movement.Duration);
                        });
                }

                if (applyMaterials && battler is not null && battler.GetComponent<SpriteRenderer>() is SpriteRenderer renderer )
                {
                    foreach (var material in Materials)
                    {
                        state.Events.Add(material.TriggerTime,
                            () => renderer.material = material.Material);
                    }
                }
                return state;
            }
        }

        [Serializable]
        public class AudioEvent
        {
            [field: SerializeField] public AudioClip Clip { get; set; }
            [field: SerializeField, AllowNesting(), MinValue(0f), MaxValue(0.9999f)] public float TriggerTime { get; set; }
        }

        [Serializable]
        public class PositionEvent
        {
            [field: SerializeField] public Movement Movement { get; set; }
            [field: SerializeField, AllowNesting(), MinValue(0f), MaxValue(0.9999f)] public float TriggerTime { get; set; }
        }
        
        [Serializable]
        public class MaterialEvent
        {
            [field: SerializeField] public Material Material { get; set; }
            [field: SerializeField, AllowNesting(), MinValue(0f), MaxValue(0.9999f)] public float TriggerTime { get; set; }

            private bool IsNotNull(Material material)
            {
                return material != null;
            }
        }
}