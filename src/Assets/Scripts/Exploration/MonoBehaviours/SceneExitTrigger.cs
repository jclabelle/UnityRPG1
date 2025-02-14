using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Assertions;

namespace Exploration.Triggers
{
    [RequireComponent(typeof(BoxCollider2D))]

    public class SceneExitTrigger : MonoBehaviour, IEventProbePassive, IDataManager, IWorldData
    {
        [SerializeField] private Vector3 spawnPoint;
        [SerializeField] private string sceneName;
        private WorldDataController worldData;
        private DataManager dataMngr;


        public Vector3 SpawnPoint
        {
            get => spawnPoint;
            set => spawnPoint = value;
        }

        public DataManager DataMngr
        {
            get => dataMngr;
            set => dataMngr = value;
        }

        public string SceneName
        {
            get => sceneName;
            set => sceneName = value;
        }

        public WorldDataController WorldData
        {
            get => worldData;
            set => worldData = value;
        }

        public void EventProbePassive(GameObject player)
        {
            DataMngr.Player.NextSpawnPosition = spawnPoint;
            WorldData.SaveScenePersistentData();
            SceneManager.LoadScene(SceneName);
        }

        private void Awake()
        {
            Assert.IsFalse(string.IsNullOrEmpty(SceneName),
                $"{SceneManager.GetActiveScene().name}, {this.GetType()}, {this.name}: SceneName is Empty or Null");
        }

        // Start is called before the first frame update
        void Start()
        {
            (this as IDataManager).SetDataManager();
            (this as IWorldData).SetWorldData();
        }

        public void EventProbeLaunched(GameObject player)
        {
            throw new NotImplementedException();
        }
    }
}