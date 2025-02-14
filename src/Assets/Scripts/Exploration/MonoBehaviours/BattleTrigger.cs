using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using BattleV3;
using UnityEngine.Assertions;


namespace Exploration.Triggers
{
    [RequireComponent(typeof(BoxCollider2D))]

    public class BattleTrigger : PersistentMonobehaviour, IDataManager, IWorldData, IEventProbePassive
    {
        [SerializeField] Sprite battleBackground;
        [SerializeField] BattleEncounter triggeredEncounter;
        private WorldDataController worldData;

        DataManager dataManager;
        [SerializeField] private bool isDone;

        public DataManager DataMngr
        {
            get => dataManager;
            set => dataManager = value;
        }

        public bool IsDone
        {
            get => isDone;
            set => isDone = value;
        }

        public BattleEncounter TriggeredEncounter
        {
            get => triggeredEncounter;
            set => triggeredEncounter = value;
        }

        public WorldDataController WorldData
        {
            get => worldData;
            set => worldData = value;
        }

        private void Awake()
        {
            Assert.IsTrue(TriggeredEncounter is BattleEncounter,
                $"{SceneManager.GetActiveScene().name}, {this.GetType()}, {this.name}: triggeredEncounter not set.");
        }

        // Start is called before the first frame update
        void Start()
        {
            (this as IDataManager).SetDataManager();
            (this as IWorldData).SetWorldData();

            // If no battleBG is set, try to default to the Map's.
            if (battleBackground is null)
                if (FindObjectOfType<ExplorationSceneController>() is ExplorationSceneController gsc)
                    if (gsc.Map is ExplorationSceneData gsd)
                        if (gsd.BattleBackground is Sprite bbg)
                            battleBackground = bbg;
        }

        // Update is called once per frame
        void Update()
        {

        }

        void IEventProbePassive.EventProbePassive(GameObject player)
        {
            if (IsDone is false)
            {

                IsDone = true;
                DataMngr.Player.NextSpawnPosition = player.transform.position;
                DataMngr.Player.CurrentWorldScene = SceneManager.GetActiveScene().name;

                WorldData.SetNextBattle(TriggeredEncounter, battleBackground);
                WorldData.SaveScenePersistentData();

                StartCoroutine(LoadBattleSceneAsync());
            }
        }

        IEnumerator LoadBattleSceneAsync()
        {
            Debug.Log("Loading scene async");
            AsyncOperation asyncload = SceneManager.LoadSceneAsync("BattleV3");
            while (!asyncload.isDone)
            {
                yield return null;
            }
        }

        public override Dictionary<string, string> GetPersistentData()
        {
            Dictionary<string, string> pData = new Dictionary<string, string>();

            pData.Add($"{nameof(IsDone)}", Convert.ToString(IsDone));

            return pData;


        }

        public override void SetPersistentData(Dictionary<string, string> pData)
        {
            IsDone = Convert.ToBoolean(pData[nameof(IsDone)]);
        }
    }

}