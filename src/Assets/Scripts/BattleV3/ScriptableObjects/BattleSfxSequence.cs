using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using NaughtyAttributes;
using Animancer;

namespace BattleV3.Animations
{

        [CreateAssetMenu(menuName = "BattleSFXSequence")]

        public class BattleSfxSequence : ScriptableObject
        {
            [field: SerializeField] public List<SfxTimeline> SfxTimelines { get; set; }
            private int iterator;
            public bool HasEndTime => SfxTimelines.All(timeline => !timeline.IsInfiniteLoop);

            public int LastToPlay
            {
                get
                {
                    return FindLastToPlay((FindLengths()));
                }
            }

            public enum EPlayType
            {
                PlayAtCenterOfScreen,
                PlayAtCenterOfBattle,
                PlayOnAllTargets,
                PlayOnPrimaryTarget,
                PlayOnUser,
                PlayOnAllMiss,
                PlayOnAllHit,
                PlayOnAllHitExcludePrimary,
                PlayOnAllMissExcludePrimary,
            }

            // Start is called before the first frame update
            void Start()
            {
            }

            // Update is called once per frame
            void Update()
            {

            }

            public void Play(int index, AnimancerComponent animancer, AudioSource audioPlayer,
                GameObject gObject, System.Action<AnimancerState, bool> onLoopEnd, Battler battler = null, bool applyMaterials = true)
            {
                var timeline = SfxTimelines[index];
                var state = timeline.BattleSfx.MakeAnimancerState(animancer, audioPlayer, gObject, 
                    timeline.IsInfiniteLoop, battler, applyMaterials);
                
                state.Events.Add(0.9999999f, () => onLoopEnd(state, timeline.ResetTransformOnLoop));

            }
            
            private List<(int, float)> FindLengths()
            {
                List<(int, float)> sfxDurations = new List<(int, float)>();
                
                for (int i = 0; i < SfxTimelines.Count; i++)
                {
                    sfxDurations.Add((i,SfxTimelines[i].EndTime ));
                }

                return sfxDurations;
            }

            private int FindLastToPlay(List<(int, float)> sfxDurations)
            {
                int indexOfLastSfx = 0;
                float longest = 0f;
                foreach (var sfxDuration in sfxDurations)
                {
                    if (sfxDuration.Item2 > longest)
                    {
                        indexOfLastSfx = sfxDuration.Item1;
                    }
                }

                return indexOfLastSfx;
            }
            
            [Serializable]
            public class SfxTimeline
            {
                // Trigger times are based on a local time in seconds. For example, in a TriggeredSFX
                // the local time (second zero) is the moment when the monobehaviour was instantiated ( StartOfLocalTime ) 
                // 
                [field: SerializeField] public float TriggerTime { get; set; }
                [field: SerializeField] public bool IsInfiniteLoop { get; set; }
                [field: SerializeField, AllowNesting, MinValue(1)] public int LoopsMax { get; set; } = 1;
                [field: SerializeField] public bool ResetTransformOnLoop { get; set; } = true;
                [field: SerializeField, Expandable] public BattleSfx BattleSfx { get; set; }
                [field: SerializeField] public Vector3 SpawnOffset { get; set; } = new Vector3(0, 0, -2);
                [field: SerializeField] public BattleSfxSequence.EPlayType PlayType { get; set; }
                public float EndTime => LoopsMax * BattleSfx.Clip.length + TriggerTime;
            }


        }
        



}