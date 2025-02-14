using System.Collections;
using System.Linq.Expressions;
using Data.Gameplay;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Quests;

public class DataManager : MonoBehaviour
{
    [SerializeField][InspectorName("Database")]private Database db;
    public Database DB { get => db; set => db = value; }

    private PlayerData player;
    public PlayerData Player { get => player; set => player = value; }

    private InventoryManagement playerInventory;
    public InventoryManagement PlayerInventory { get => playerInventory; set => playerInventory = value; }

    private QuestManagement playerQuests;
    public QuestManagement PlayerQuests { get => playerQuests; set => playerQuests = value; }

    private RewardsManagement playerRewards;
    public RewardsManagement PlayerRewards { get => playerRewards; set => playerRewards = value; }


    private string saveSlotOne = "Save Slot One";
    private string saveSlotTwo = "Save Slot Two";
    private string saveSlotThree = "Save Slot Three";
    private string saveSlotFour = "Save Slot Four";
    private string saveSlotFive = "Save Slot Five";
    private string saveSlotSix = "Save Slot Six";
    private string saveSlotSeven = "Save Slot Seven";
    private string saveSlotEight = "Save Slot Eight";

    public string SaveSlotOne { get => saveSlotOne; set => saveSlotOne = value; }
    public string SaveSlotTwo { get => saveSlotTwo; set => saveSlotTwo = value; }
    public string SaveSlotThree { get => saveSlotThree; set => saveSlotThree = value; }
    public string SaveSlotFour { get => saveSlotFour; set => saveSlotFour = value; }
    public string SaveSlotFive { get => saveSlotFive; set => saveSlotFive = value; }
    public string SaveSlotSix { get => saveSlotSix; set => saveSlotSix = value; }
    public string SaveSlotSeven { get => saveSlotSeven; set => saveSlotSeven = value; }
    public string SaveSlotEight { get => saveSlotEight; set => saveSlotEight = value; }

    private string activeSaveSlot = System.String.Empty;
    public string ActiveSaveSlot { get => activeSaveSlot; set => activeSaveSlot = value; }

    private void Awake()
    {
        //Debug.Log("DataManager Awake");
        DontDestroyOnLoad(this);
        Player = new PlayerData();
        PlayerInventory = new InventoryManagement();
        PlayerInventory.SetDataManager(this);
        PlayerRewards = new RewardsManagement();
        PlayerRewards.SetDataManager(this);
        PlayerQuests = new QuestManagement();
        PlayerQuests.SetDataManager(this);


    }
    private void OnDestroy()
    {
        Debug.Log("Data Manager Destroyed");
    }
    private void Start()
    {



    }

    private void Update()
    {

    }

    public T Search<T>(string name) where T : UnityEngine.Object
    {

        if (DB.BattleEncountersDB.all != null)
        {
            if (DB.BattleEncountersDB.all.Find(item => item.ToString().Trim() == name))
            {
                var asset = DB.BattleEncountersDB.all.Find(item => item.ToString() == name);

                return (T)(UnityEngine.Object)asset;
            }
        }

        if (DB.ItemsDB.all != null)
        {
            if (DB.ItemsDB.all.Find(item => item.ToString().Trim() == name))
            {
                var asset = DB.ItemsDB.all.Find(item => item.ToString() == name);

                return (T)(UnityEngine.Object)asset;
            }
        }

        if (DB.AbilitiesDB.all != null)
        {
            if (DB.AbilitiesDB.all.Find(item => item.ToString().Trim() == name))
            {
                var asset = DB.AbilitiesDB.all.Find(item => item.ToString() == name);

                return (T)(UnityEngine.Object)asset;
            }
        }

        if (DB.ReactionsDB.all != null)
        {
            if (DB.ReactionsDB.all.Find(item => item.ToString().Trim() == name))
            {
                var asset = DB.ReactionsDB.all.Find(item => item.ToString() == name);

                return (T)(UnityEngine.Object)asset;
            }
        }

        if (DB.QuestsDB.all != null)
        {
            if (DB.QuestsDB.all.Find(item => item.ToString().Trim() == name))
            {
                var asset = DB.QuestsDB.all.Find(item => item.ToString() == name);

                return (T)(UnityEngine.Object)asset;
            }
        }

        if (DB.ObjectivesDB.all != null)
        {
            if (DB.ObjectivesDB.all.Find(item => item.ToString().Trim() == name))
            {
                var asset = DB.ObjectivesDB.all.Find(item => item.ToString() == name);

                return (T)(UnityEngine.Object)asset;
            }
        }


        return default(T);
    }

    public void NewGame(string playerName, string saveSlot)
    {
        Player = Player.New(this);
        Player.PlayerName = playerName;
        var playerData = Player.GetSaveGameJson();
        ActiveSaveSlot = saveSlot;
        SavePlayerToPlayerPrefs(playerData, saveSlot);
    }
    public void SaveGame(string saveSlot, PlayerData playerData)
    {
        var playerJson = Player.GetSaveGameJson();
        SavePlayerToPlayerPrefs(playerJson, saveSlot);
    }

    public void LoadGame(string saveSlot, DataManager dataManager)
    {
        ActiveSaveSlot = saveSlot;
        var saveGameJson = LoadPlayerFromPlayerPrefs(saveSlot);
        Player.SetFromGameSaveJson(saveGameJson, dataManager);
        Debug.Log(saveGameJson);
    }

    private void SavePlayerToPlayerPrefs(JObject playerData, string saveSlot)
    {
        if (playerData is JObject saveData)
        {
            string saveJson = JsonConvert.SerializeObject(saveData, Formatting.Indented);
            Debug.Log($"Saving:\n{saveJson}");
            PlayerPrefs.SetString(saveSlot.Trim(), saveJson);
        }
        else
        {
            Debug.Log("WARNING: Wrong file provided as argument to Save().");
        }
    }

    private JObject LoadPlayerFromPlayerPrefs(string saveSlot)
    {
        ActiveSaveSlot = saveSlot;
        if (PlayerPrefs.HasKey(saveSlot.Trim()))
        {
            var loadJson = PlayerPrefs.GetString(saveSlot.Trim());
            Debug.Log($"Loaded:\n{loadJson}");

            return JsonConvert.DeserializeObject<JObject>(loadJson);

        }
        else
        {
            Debug.Log("WARNING: Failed loading file. Returning NULL.");
            return null;
        }

    }

}