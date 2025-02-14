using System.Collections;
using System.Collections.Generic;
using BattleV3;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

// Goes on a Map Manager object
namespace Exploration
{
    [RequireComponent(typeof(AudioSource))]

    public class ExplorationSceneController : MonoBehaviour, IDataManager, IWorldData
    {
        [SerializeField] private ExplorationSceneData map;
        [SerializeField] private GameObject transitionTrigger;
        [SerializeField] private float musicVolume = 0.5f;


        private AudioSource sfx;
        private bool battleIsLoading;
        private DataManager dataMngr;
        private WorldDataController worldData;
        [SerializeField] private EncounterZone activeEncounterZone;

        public ExplorationSceneData Map
        {
            get => map;
            set => map = value;
        }

        public AudioSource SFX
        {
            get => sfx;
            set => sfx = value;
        }

        public GameObject TransitionTrigger
        {
            get => transitionTrigger;
            set => transitionTrigger = value;
        }

        public DataManager DataMngr
        {
            get => dataMngr;
            set => dataMngr = value;
        }

        public float MusicVolume
        {
            get => musicVolume;
            set => musicVolume = value;
        }

        public WorldDataController WorldData
        {
            set => worldData = value;
            get => worldData;
        }

        public bool BattleIsLoading
        {
            get => battleIsLoading;
            set => battleIsLoading = value;
        }

        public EncounterZone ActiveEncounterZone
        {
            get => activeEncounterZone;
            set => activeEncounterZone = value;
        }


        private void Awake()
        {
            Assert.IsTrue(Map is ExplorationSceneData,
                $"GameSceneData not set in GameSCeneController in scene {SceneManager.GetActiveScene().name}");

        }

        // Play music
        void Start()
        {
            (this as IDataManager).SetDataManager();
            (this as IWorldData).SetWorldData();
            SFX = gameObject.GetComponent<AudioSource>();
            PlayMusic();
        }

        // Update is called once per frame
        void Update()
        {
            SFX.volume = MusicVolume;
        }

        public void SetActiveEncounterZone(EncounterZone zone)
        {
            activeEncounterZone = zone;
            Debug.Log($"Active encounter zone set to {zone.gameObject.name}");
        }

        public void RemoveActiveEncounterZone(EncounterZone zone)
        {
            if (zone == ActiveEncounterZone)
            {
                Debug.Log($"Removing Active encounter zone  {zone.gameObject.name}");
                ActiveEncounterZone = null;
            }
        }

        // Used by the Player Controller every time the player takes a step
        // Has a random chance of starting a random encounter 
        public void RollRandomEncounterChance()
        {
            if (BattleIsLoading is false)
            {

                float rnd = Random.Range(0, 101);
                if (ActiveEncounterZone is EncounterZone zone)
                {
                    if (rnd < zone.EncounterChancePerStep)
                    {
                        StartBattle(zone);

                    }
                }
                else if (Map.RandomEncounters is BattleEncounter[] encounters &&
                         Map.RandomEncounters.Length > 0)
                {
                    if (rnd < Map.EncounterChancePerStep)
                    {
                        StartBattle();
                    }
                }

            }

        }

        void StartBattle(EncounterZone zone)
        {
            BattleIsLoading = true;
            DataMngr.Player.NextSpawnPosition = GameObject.Find("Player").transform.position;
            DataMngr.Player.CurrentWorldScene = SceneManager.GetActiveScene().name;
            WorldData.SetNextBattle(zone.GetEncounter(), zone.BattleBackground);
            WorldData.SaveScenePersistentData();
            StartCoroutine(LoadBattleSceneAsync());
        }

        void StartBattle()
        {
            //todo: Add Encounter Table to MapSceneData so we don't always fight against RandomEncounters[0]
            BattleIsLoading = true;
            DataMngr.Player.NextSpawnPosition = GameObject.Find("Player").transform.position;
            DataMngr.Player.CurrentWorldScene = SceneManager.GetActiveScene().name;
            WorldData.SetNextBattle(Map.RandomEncounters[0], Map.BattleBackground);
            WorldData.SaveScenePersistentData();
            StartCoroutine(LoadBattleSceneAsync());
        }

        void PlayMusic()
        {
            if (Map != null && Map.Music != null)
            {
                SFX.clip = Map.Music;
                SFX.volume = MusicVolume;
                SFX.Play();
            }
        }

        IEnumerator LoadBattleSceneAsync()
        {
            AsyncOperation loadSceneAsync = SceneManager.LoadSceneAsync("BattleV3");
            while (!loadSceneAsync.isDone)
            {
                yield return null;
            }
        }

    }
}
