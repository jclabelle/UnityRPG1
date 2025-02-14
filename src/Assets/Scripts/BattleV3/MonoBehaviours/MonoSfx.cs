using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Animancer;
using UnityEngine;

namespace BattleV3.Animations
{
    
    [RequireComponent(typeof(Animator), typeof(AnimancerComponent), typeof(AudioSource))]
    public class MonoSfx : MonoBehaviour
    {
        public AnimancerComponent Animancer { get; set; }
        public AudioSource AudioPlayer { get; set; }
        private Animator Animator { get; set; }
        private SpriteRenderer Renderer { get; set; }
        public int LoopsMax { get; set; }
        public int LoopsCompleted { get; set; }
        private System.Action SfxSequenceCallDestroy { get; set; }
        public bool IsLast { get; set; }
        public bool IsDonePlaying => LoopsCompleted == LoopsMax;
        public Vector3 OriginalPosition { get; set; } 
        public Quaternion OriginalRotation { get; set; } 
        public Vector3 OriginalScale { get; set; } 

        private void Awake()
        {
            // Save original transform in case we need to revert changes, such as
            // reset the position at the end of a looping animation.
            var transform1 = transform;

        }

        // Start is called before the first frame update
        void Start()
        {
            if(Animancer is null)
                Animancer = GetComponent<AnimancerComponent>();
            if(Animator is null)
                Animator = GetComponent<Animator>();
            if(AudioPlayer is null)
                AudioPlayer = GetComponent<AudioSource>();
            if (Renderer is null)
                Renderer = GetComponent<SpriteRenderer>();

            if (Renderer is not null)
            {
                Renderer.sortingLayerName = BattleFX.SfxSortingLayerName;
                Renderer.sortingOrder = BattleFX.SfxSortingOrder;
            }
        }

        public void OnLoopEnd(AnimancerState state, bool resetTransform = true)
        {
            LoopsCompleted++;
            if (LoopsCompleted == LoopsMax)
            {
                state.Stop();
            }
            else
            {
                state.NormalizedTime = 0;
                if (resetTransform is true)
                {
                    // var transform1 = transform;
                    transform.position = OriginalPosition;
                    transform.localScale = OriginalScale;
                    transform.rotation = OriginalRotation;
                }
            }
            
        }

        // Update is called once per frame
        void Update()
        {
            if (IsLast && IsDonePlaying && !AudioPlayer.isPlaying)
                SfxSequenceCallDestroy();
        }

        public void Init(System.Action callDestroy, Vector3 position, Vector3 scale, Quaternion rotation)
        {
            Init(callDestroy, position);
            OriginalScale = scale;
            OriginalRotation = rotation;

        }
        
        public void Init(System.Action callDestroy, Vector3 position)
        {
            Animancer = GetComponent<AnimancerComponent>();
            Animator = GetComponent<Animator>();
            AudioPlayer = GetComponent<AudioSource>();
            SfxSequenceCallDestroy = callDestroy;
            
            OriginalPosition = position;
            OriginalScale = Vector3.one;
            OriginalRotation = default;

            transform.position = position;
        }

        public void CallDestroy()
        {
            UnityEngine.Object.Destroy(gameObject);

        }

    }
}