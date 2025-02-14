using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BattleV3;
using UnityEngine;
using Animancer;


namespace BattleV3.Animations
{
    [RequireComponent(typeof(Animator), typeof(Animancer.AnimancerComponent), typeof(AudioSource))]
    public class MonoSfxPlayer : MonoBehaviour
    {
        public float StartDelay { get; set; }
        public float StartOfLocalTime { get; set; } // The moment when local time starts
        [field: SerializeField] public BattleSfxSequence SfxSequence { get; set; }
        private int Iterator { get; set; }
        private List<MonoSfx> MonoSfxActives { get; set; } = new List<MonoSfx>();
        public MonoAbility.ParticipatingBattlers Participants { get; set; }

        private bool IsPlaying
        {
            get;
            set;
        }


        void Start()
        {

            StartOfLocalTime = Time.time;
        }

        // Update is called once per frame
        void Update()
        {
            IsPlaying = Time.time > StartOfLocalTime + StartDelay;
               
            // Try to start playing a BattleSfx every frame.
            if (IsPlaying && Iterator < SfxSequence.SfxTimelines.Count)
            {
                if (Time.time > StartOfLocalTime + SfxSequence.SfxTimelines[Iterator].TriggerTime)
                {
                    PlayBattleSfx(Iterator, SfxSequence.SfxTimelines[Iterator].PlayType);
                    Iterator++;
                }
            }

        }

        private void PlayBattleSfx(int iterator)
        {
            var gObject = new GameObject($"MonoSfx {SfxSequence.SfxTimelines[iterator].BattleSfx.name}",
                typeof(MonoSfx));
            gObject.AddComponent<SpriteRenderer>();
            var monoSfx = gObject.GetComponent<MonoSfx>();
            MonoSfxActives.Add(monoSfx);

            switch (SfxSequence.SfxTimelines[iterator].PlayType)
            {
                    case BattleSfxSequence.EPlayType.PlayAtCenterOfBattle :
                    {
                        monoSfx.Init(CallDestroy, Battle.GetCenterPositionOfBattlefield());
                        break;
                    }
                    case BattleSfxSequence.EPlayType.PlayAtCenterOfScreen :
                    {
                        if (Camera.main != null)
                            monoSfx.Init(CallDestroy, Camera.main.ScreenToWorldPoint(Vector3.zero));
                        else
                            monoSfx.Init(CallDestroy, Battle.GetCenterPositionOfBattlefield());
                        break;
                    }
                        
            }
            
            SfxSequence.Play(iterator, monoSfx.Animancer, monoSfx.AudioPlayer, gObject, monoSfx.OnLoopEnd, Participants.User, true);
        }

        private void PlayBattleSfx(int iterator, BattleSfxSequence.EPlayType playType)
        {
            switch (playType)
            {
                case BattleSfxSequence.EPlayType.PlayOnUser:
                {
                    if (Participants.User)
                        PlayBattleSfx(iterator, new List<Battler>(){Participants.User});
                    break;
                }

                case BattleSfxSequence.EPlayType.PlayOnPrimaryTarget:
                {
                    if (Participants.PrimaryTarget)
                        PlayBattleSfx(iterator, new List<Battler>(){Participants.PrimaryTarget});
                    break;
                }

                case BattleSfxSequence.EPlayType.PlayOnAllHit:
                {
                    if(Participants.AllHitTargets.Count > 0)
                        PlayBattleSfx(iterator, Participants.AllHitTargets );
                    break;
                }
                
                case BattleSfxSequence.EPlayType.PlayOnAllHitExcludePrimary:
                {
                    if(Participants.AllHitsExcludePrimary.Count > 0)
                        PlayBattleSfx(iterator, Participants.AllHitsExcludePrimary );
                    break;
                }
                
                case BattleSfxSequence.EPlayType.PlayOnAllMiss:
                {
                    if(Participants.AllMissedTargets.Count > 0)
                        PlayBattleSfx(iterator, Participants.AllMissedTargets, false );
                    break;
                }
                
                case BattleSfxSequence.EPlayType.PlayOnAllMissExcludePrimary:
                {
                    if(Participants.AllHitsExcludePrimary.Count > 0)
                        PlayBattleSfx(iterator, Participants.AllMissExcludePrimary, false );
                    break;
                }
                
                case BattleSfxSequence.EPlayType.PlayOnAllTargets:
                {
                    if(Participants.AllTargets.Count > 0)
                        PlayBattleSfx(iterator, Participants.AllTargets );
                    break;
                }
                
                case BattleSfxSequence.EPlayType.PlayAtCenterOfBattle:
                {
                    PlayBattleSfx(iterator);
                    break;
                }
                
                case BattleSfxSequence.EPlayType.PlayAtCenterOfScreen:
                {
                    PlayBattleSfx(iterator);
                    break;
                }
                
                
            }
        }
        
        private void PlayBattleSfx(int iterator, List<Battler> battlers, bool applyShaderOnBattlers = true)
        {
            foreach (var battler in battlers)
            {
                var gObject = new GameObject($"MonoSfx {SfxSequence.SfxTimelines[iterator].BattleSfx.name}",
                    typeof(MonoSfx));
                gObject.AddComponent<SpriteRenderer>();
                var monoSfx = gObject.GetComponent<MonoSfx>();
                
                var position = battler.transform.position;
                position += SfxSequence.SfxTimelines[iterator].SpawnOffset;
                
                monoSfx.Init(CallDestroy, position);
                monoSfx.LoopsMax = SfxSequence.SfxTimelines[iterator].LoopsMax;
                
                MonoSfxActives.Add(monoSfx);
                
                if(SfxSequence.HasEndTime)
                    monoSfx.IsLast = SfxSequence.LastToPlay == iterator;
                
                SfxSequence.Play(iterator, monoSfx.Animancer, monoSfx.AudioPlayer, gObject, monoSfx.OnLoopEnd, battler, applyShaderOnBattlers);
            }
        }

        private void PlaySfxSequence(BattleSfxSequence.SfxTimeline timeline, GameObject gObject)
        {
   
        }
        
        public void CallDestroy()
        {
            foreach(var monoSfx in MonoSfxActives)
                monoSfx.CallDestroy();
            
            UnityEngine.GameObject.Destroy(gameObject);
        }

        public static MonoSfxPlayer Make(Animations.BattleSfxSequence sfx, Vector3 position)
        {
            GameObject triggeredSfx = new GameObject($"TriggedSfx {sfx.name}",typeof(MonoSfxPlayer));
            triggeredSfx.transform.position = position;
            triggeredSfx.GetComponent<MonoSfxPlayer>().SfxSequence = sfx;
            return triggeredSfx.GetComponent<MonoSfxPlayer>();
        }
        
        public static MonoSfxPlayer Make(Animations.BattleSfxSequence sfx, MonoAbility.ParticipatingBattlers participants)
        {
            GameObject triggeredSfx = new GameObject($"TriggedSfx {sfx.name}",typeof(MonoSfxPlayer));
            triggeredSfx.transform.position = new Vector3(0,0,-1);
            var monoSfx = triggeredSfx.GetComponent<MonoSfxPlayer>();
            monoSfx.SfxSequence = sfx;
            monoSfx.Participants = participants; 
            return triggeredSfx.GetComponent<MonoSfxPlayer>();
        }
        
        
     
    }
}