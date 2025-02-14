using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace BattleV3
{
    public class BattleFX : MonoBehaviour, IBattleEffects
    {
        public IBattleEffects BattleEffects => this;

        public static string PlayerSortingLayerName { get; set; } = "Player";
        public static int PlayerSortingOrder { get; set; } = 0;
        public static string SfxSortingLayerName { get; set; } = "SfxBelowSky";
        public static int SfxSortingOrder { get; set; }
        public static Material BattlerDefaultMaterial { get; set; }
        [field: SerializeField] private Material DefaultBattlerMaterial { get; set; }

        private void Awake()
        {
            BattlerDefaultMaterial = DefaultBattlerMaterial;
        }

        private void Start()
        {
        }

        public bool AreTriggeredSfxDone()
        {
            return GameObject.FindObjectsOfType<Animations.MonoSfxPlayer>().Length <= 0;
        }
    }

    public interface IBattleEffects
    {
        public bool AreTriggeredSfxDone();
    }


}