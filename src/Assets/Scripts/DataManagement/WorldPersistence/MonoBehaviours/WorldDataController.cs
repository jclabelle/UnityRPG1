using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using WorldPersistence;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class WorldDataController : MonoBehaviour
{
    WorldData wData;
    Dictionary<string, GameObject> persistentPrefabsDB;
    string PATH_TO_PREFABS = "Prefabs/";
    private Sprite nextBattleBackground;
    private BattleV3.BattleEncounter nextBattleEncounter;

    public WorldData WData { get => wData; set => wData = value; }
    public Sprite NextBattleBackground { get => nextBattleBackground; set => nextBattleBackground = value; }
    public BattleV3.BattleEncounter NextBattleEncounter { get => nextBattleEncounter; set => nextBattleEncounter = value; }

    private void Awake()
    {
        DontDestroyOnLoad(this);

        WData = new WorldData();
        WData.Init();
        WData.Load();

        LoadGamePrefabs();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    // Start is called before the first frame update
    void Start()
    {
       


    }

    public void SetNextBattle(BattleV3.BattleEncounter encounter, Sprite background)
    {
        NextBattleBackground = background;
        NextBattleEncounter = encounter;
    }
    void LoadScenePersistentData()
    {
        System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
        watch.Start();

        // If we have already visited this scene, destroy existing Persistent prefabs objects and load from WorldData
        if (WData.IsSceneInWorldData(SceneManager.GetActiveScene().name) is true)
        {

            var allPersistent = FindObjectsOfType<PersistenceController>();
            foreach (var po in allPersistent)
            {
                po.DestroyParent();
            }

            var pObjectsList = WData.GetPObjectsInScene(SceneManager.GetActiveScene().name);
            foreach(var pObjectData in pObjectsList)
            {
                Dictionary<string, Dictionary<string, string>> deSerializedData = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(pObjectData.SerializedData);

                var prefab = Instantiate(persistentPrefabsDB[deSerializedData["PersistenceController"]["prefabGuid"]]);

                var persistenceController = prefab.GetComponent<PersistenceController>();
                persistenceController.SetPersistentData(pObjectData);
       
            }
        }
        watch.Stop();
        Debug.Log($"Loadworld:\t\t\t " + watch.Elapsed.ToString().Remove(0, 8));

    }

    // Updates persistent object data on the datatables. Not a commit to disk. Use CommitWorldData to save the datatables to disk.
    public void SaveScenePersistentData()
    {
 

        var allPersistent = FindObjectsOfType<PersistenceController>();

        List<PObjectData> pObjectList = new List<PObjectData>();
        foreach(var po in allPersistent)
        {
            pObjectList.Add(po.GetPersistentData());
        }
        WData.SetPObjectsInScene(SceneManager.GetActiveScene().name, pObjectList);

    

    }

    // Commits the datatables to disk. Permanent.
    public void CommitWorldData()
    {
        WData.Save();
    }

    // Update is called once per frame
    void Update()
    {
 
    }



    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name is not "Main Menu")
        {
            LoadScenePersistentData();
        }

    }

    public Dictionary<string, GameObject> LoadGamePrefabs()
    {
        persistentPrefabsDB = new Dictionary<string, GameObject>();
        var prefabs = Resources.LoadAll<GameObject>(PATH_TO_PREFABS);

        foreach(var p in prefabs)
        {
            if(p.GetComponent<PersistenceController>() is PersistenceController pc)
            {
                persistentPrefabsDB.Add(pc.PrefabGuid, p);
            }
        }
        return persistentPrefabsDB;
    }



    void TestIn()
    {
        var outdata = WData.GetPObjectsInScene(SceneManager.GetActiveScene().name);
        foreach(var pobject in outdata)
        {
            Debug.Log(pobject.SerializedData);
        }

        PObjectData po = new PObjectData("64a671b2-7dc3-4512-928d-1de927a5e48b", "WAS SOMETHING SOMETHING");
        List<PObjectData> list = new List<PObjectData>();
        list.Add(po);
        WData.SetPObjectsInScene(SceneManager.GetActiveScene().name, list);
        outdata = WData.GetPObjectsInScene(SceneManager.GetActiveScene().name);
        foreach (var pobject in outdata)
        {
            Debug.Log(pobject.SerializedData);
        }
        WData.Save();

    }

    
}