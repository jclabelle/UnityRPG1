using System.Collections;
using System.Collections.Generic;
using BattleV3;
using UnityEngine;
using UnityEngine.SceneManagement;

// Default data about scenes: random encounters, battle backgrounds, music etc
namespace Exploration
{
    [CreateAssetMenu (menuName = "Exploration/SceneData")]
    public class ExplorationSceneData : ScriptableObject
    {
        [SerializeField] private BattleEncounter[] randomEncounters;
        [SerializeField] public int EncounterChancePerStep { get; set; }
        [SerializeField] private Sprite battleBackground;
        [SerializeField] private AudioClip music;

        public Sprite BattleBackground
        {
            get => battleBackground;
            set => battleBackground = value;
        }

        public AudioClip Music
        {
            get => music;
            set => music = value;
        }

        public BattleEncounter[] RandomEncounters
        {
            get => randomEncounters;
            set => randomEncounters = value;
        }
    }
}
