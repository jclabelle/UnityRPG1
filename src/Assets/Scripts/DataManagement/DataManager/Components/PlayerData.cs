using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Quests;

[System.Serializable]
public class PlayerData : ISavePlayerData
{
    /// <summary>
    /// Scene transitions
    /// </summary>
    [System.NonSerialized] private Vector3 nextSpawnPosition; // Next player position when entering a non-battle scene.
    public Vector3 NextSpawnPosition { get => nextSpawnPosition; set => nextSpawnPosition = value; }

    [SerializeField] private string currentWorldScene; // Current non-battle scene. Defaults to Village
    public string CurrentWorldScene { get => currentWorldScene; set => currentWorldScene = value; }

    /// <summary>
    /// Ressources, Abilities and Reactions.
    /// Ressources are set at the end of combat by BattleManager in BattleComplete
    /// </summary>

    [SerializeField] private IAdditiveStats.StatModifiers baseStats;
    private IAdditiveStats.StatModifiers BaseStats { get => baseStats; set => baseStats = value; }

    public IAdditiveStats.StatModifiers MaxStats { get => BaseStats + GetEquipmentModifiers(); }

    [SerializeField] private IAdditiveStats.StatModifiers currentStats;
    public IAdditiveStats.StatModifiers CurrentStats { get => currentStats; set => currentStats = value; }

    [SerializeField] List<AbilityV2> abilities;
    [SerializeField] List<ReactionV2> reactions;
    public List<AbilityV2> Abilities { get => abilities; set => abilities = value; }
    public List<ReactionV2> Reactions { get => reactions; set => reactions = value; }
    [field: SerializeField] public AbilityV2 BasicAttack { get; set; }
    public Dictionary<string, ReactionV2> EquippedReactions { get ; set ; }

    /// <summary>
    /// QuestController
    /// </summary>
    [SerializeField] private List<Quest> completedQuests;
    public List<Quest> CompletedQuests { get => completedQuests; set => completedQuests = value; }
    [SerializeField] private Dictionary<Quest, List<Objective>> activeQuests;
    public Dictionary<Quest, List<Objective>> ActiveQuests { get => activeQuests; set => activeQuests = value; }

    /// <summary>
    /// Character Level Progression
    /// </summary>
    [SerializeField] int xp;
    public int XP { get => xp; set => xp = value; }
    string playerName;
    public string PlayerName { get => playerName; set => playerName = value; }


    /// <summary>
    /// Inventory
    /// </summary>

    [SerializeField] List<IInventoryItem> items;
    public List<IInventoryItem> Items
    {
        get => items;
        set => items = value;
    }

    [SerializeField] int gold;
    public int Gold { get => gold; set => gold = value; }

    public Armor EquippedArmor { get => equippedArmor; set { OnEquipUpdateCurrentStats(EquippedArmor, value); equippedArmor = value; CurrentStats.ClampAll(MaxStats); } }
    public Boots EquippedBoots { get => equippedBoots; set { OnEquipUpdateCurrentStats(EquippedBoots, value); equippedBoots = value; CurrentStats.ClampAll(MaxStats); } }
    public Gloves EquippedGloves { get => equippedGloves; set { OnEquipUpdateCurrentStats(equippedGloves, value); equippedGloves = value; CurrentStats.ClampAll(MaxStats); } }
    public Helmet EquippedHelmet { get => equippedHelmet; set { OnEquipUpdateCurrentStats(equippedHelmet, value); equippedHelmet = value; CurrentStats.ClampAll(MaxStats); } }
    public Necklace EquippedNecklace { get => equippedNecklace; set { OnEquipUpdateCurrentStats(equippedNecklace, value); equippedNecklace = value; CurrentStats.ClampAll(MaxStats); } }
    public Ring EquippedLeftRing { get => equippedLeftRing; set { OnEquipUpdateCurrentStats(equippedLeftRing, value); equippedLeftRing = value; CurrentStats.ClampAll(MaxStats); } }
    public Ring EquippedRightRing { get => equippedRightRing; set { OnEquipUpdateCurrentStats(equippedRightRing, value); equippedRightRing = value; CurrentStats.ClampAll(MaxStats); } }

    public IHandHeld EquippedRightHand
    {
        get => equippedRightHand;
        set
        {
            OnEquipUpdateCurrentStats((IAdditiveStats)equippedRightHand, (IAdditiveStats)value);
            CurrentStats.ClampAll(MaxStats);
            equippedRightHand = value;
        }
    }

    public IHandHeld EquippedLeftHand
    {
        get => equippedLeftHand;
        set
        {
            OnEquipUpdateCurrentStats((IAdditiveStats)equippedLeftHand, (IAdditiveStats)value);
            CurrentStats.ClampAll(MaxStats);
            equippedLeftHand = value;
        }
    }

    [SerializeField] Armor equippedArmor;
    [SerializeField] Boots equippedBoots;
    [SerializeField] Gloves equippedGloves;
    [SerializeField] Helmet equippedHelmet;
    [SerializeField] Necklace equippedNecklace;
    [SerializeField] Ring equippedLeftRing;
    [SerializeField] Ring equippedRightRing;

    private IHandHeld equippedLeftHand;
    private IHandHeld equippedRightHand;

    public void Init()
    {
        NextSpawnPosition = new Vector3();
        CurrentWorldScene = System.String.Empty; ;
        CurrentStats = new IAdditiveStats.StatModifiers();
        BaseStats = new IAdditiveStats.StatModifiers();
        Abilities = new List<AbilityV2>();
        Reactions = new List<ReactionV2>();
        EquippedReactions = new Dictionary<string, ReactionV2>();
        EquippedReactions.Add("SlotOne", null);
        EquippedReactions.Add("SlotTwo", null);
        EquippedReactions.Add("SlotThree", null);
        EquippedReactions.Add("SlotFour", null);
        EquippedReactions.Add("SlotFive", null);
        EquippedReactions.Add("SlotSix", null);

        CompletedQuests = new List<Quest>();
        ActiveQuests = new Dictionary<Quest, List<Objective>>();
        Items = new List<IInventoryItem>();

        SetSaveGameData();
    }

    public PlayerData New(DataManager dataManager)
    {
        PlayerData newPlayerData = new PlayerData();
        newPlayerData.InitAsNewGame(dataManager);
        return newPlayerData;
    }

    // todo: Make a final version of this
    private void InitAsNewGame(DataManager dbm)
    {
        Init();
        BaseStats = IAdditiveStats.StatModifiers.Make(100, 50, 1, 10, 10, 10, 1, 1, 1);
        CurrentStats = IAdditiveStats.StatModifiers.Make(100, 50, 1, 10, 10, 10, 1, 1, 1);

        CurrentWorldScene = "Village";
        NextSpawnPosition = new Vector3(0.5f, 0.5f, 0f);
        
        BasicAttack = dbm.Search<AbilityV2>("Basic Attack (AbilityV2)");
        
        // Abilities.Add(dbm.Search<AbilityV2>("Basic Attack (AbilityV2)"));
        Abilities.Add(dbm.Search<AbilityV2>("Blade Slash (AbilityV2)"));
        Abilities.Add(dbm.Search<AbilityV2>("WhirldWindV2 (AbilityV2)"));
        Abilities.Add(dbm.Search<AbilityV2>("Recover (AbilityV2)"));
        Abilities.Add(dbm.Search<AbilityV2>("Wand Slam (AbilityV2)"));
        Abilities.Add(dbm.Search<AbilityV2>("Axe Smash (AbilityV2)"));
        Abilities.Add(dbm.Search<AbilityV2>("Bone Rush (AbilityV2)"));
        Abilities.Add(dbm.Search<AbilityV2>("Fire Blast (AbilityV2)"));
        Abilities.Add(dbm.Search<AbilityV2>("Blade Slash (AbilityV2)"));
        Abilities.Add(dbm.Search<AbilityV2>("WhirldWindV2 (AbilityV2)"));
        Abilities.Add(dbm.Search<AbilityV2>("Recover (AbilityV2)"));
        Abilities.Add(dbm.Search<AbilityV2>("Wand Slam (AbilityV2)"));
        Abilities.Add(dbm.Search<AbilityV2>("Axe Smash (AbilityV2)"));
        Abilities.Add(dbm.Search<AbilityV2>("Bone Rush (AbilityV2)"));
        Abilities.Add(dbm.Search<AbilityV2>("Fire Blast (AbilityV2)"));
        Abilities.Add(dbm.Search<AbilityV2>("Blade Slash (AbilityV2)"));
        Abilities.Add(dbm.Search<AbilityV2>("WhirldWindV2 (AbilityV2)"));
        Abilities.Add(dbm.Search<AbilityV2>("Recover (AbilityV2)"));
        Abilities.Add(dbm.Search<AbilityV2>("Wand Slam (AbilityV2)"));
        Abilities.Add(dbm.Search<AbilityV2>("Axe Smash (AbilityV2)"));
        Abilities.Add(dbm.Search<AbilityV2>("Bone Rush (AbilityV2)"));
        Abilities.Add(dbm.Search<AbilityV2>("Fire Blast (AbilityV2)"));

        Reactions.Add(dbm.Search<ReactionV2>("Shield Block (ReactStatDecreaseV2)"));
        Reactions.Add(dbm.Search<ReactionV2>("Magic Block (ReactStatDecreaseV2)"));
        Reactions.Add(dbm.Search<ReactionV2>("Riposte (ReactStatDecreaseV2)"));
        Reactions.Add(dbm.Search<ReactionV2>("New React Stat Decrease V2 (ReactStatDecreaseV2)"));
        Reactions.Add(dbm.Search<ReactionV2>("New React Stat Decrease V2 2 (ReactStatDecreaseV2)"));
        Reactions.Add(dbm.Search<ReactionV2>("New React Stat Decrease V2 1 (ReactStatDecreaseV2)"));


        Items.Add(dbm.Search<Item>("Clothes (Armor)"));
        Items.Add(dbm.Search<Item>("Rusty Sword (Weapon)"));
        Items.Add(dbm.Search<Item>("Crystal Ring (Ring)"));
        Items.Add(dbm.Search<Item>("Saphire Necklace (Necklace)"));
        Items.Add(dbm.Search<Item>("Wing Boots (Boots)"));
        Items.Add(dbm.Search<Item>("Clothes (Armor)"));
        Items.Add(dbm.Search<Item>("Rusty Sword (Weapon)"));
        Items.Add(dbm.Search<Item>("Crystal Ring (Ring)"));
        Items.Add(dbm.Search<Item>("Saphire Necklace (Necklace)"));
        Items.Add(dbm.Search<Item>("Wing Boots (Boots)"));
        Items.Add(dbm.Search<Item>("Clothes (Armor)"));
        Items.Add(dbm.Search<Item>("Rusty Sword (Weapon)"));
        Items.Add(dbm.Search<Item>("Crystal Ring (Ring)"));
        Items.Add(dbm.Search<Item>("Saphire Necklace (Necklace)"));
        Items.Add(dbm.Search<Item>("Wing Boots (Boots)"));

        var q = dbm.Search<Quest>("Quest 1 (Quests.Quest)");
        List<Objective> obj = new List<Objective>();
        ActiveQuests.Add(q, obj);
    }

    /// <summary>
    /// Save Game
    /// </summary>
    private string saveGameName;
    private string saveGameType;

    public string SaveGameName { get => saveGameName; set => saveGameName = value; }
    public string SaveGameType { get => saveGameType; set => saveGameType = value; }

    public void SetSaveGameData()
    {
        SaveGameName = $"{this}".Trim();
        SaveGameType = $"{this.GetType()}".Trim();
    }

    private void OnEquipUpdateCurrentStats(IAdditiveStats removed, IAdditiveStats equipped)
    {
        if (removed is Equipment r)
        {
            if (equipped is Equipment e)
            {
                CurrentStats += (e.Stats - r.Stats);
            }
            else
            {
                CurrentStats -= r.Stats;
            }
        }
        else if (removed is null)
        {
            if (equipped is Equipment e)
            {
                CurrentStats += e.Stats;
            }
        }
    }

    public IAdditiveStats.StatModifiers GetEquipmentModifiers()
    {
        IAdditiveStats.StatModifiers total = new IAdditiveStats.StatModifiers();

        if (EquippedRightHand != null)
            total += EquippedRightHand.GetAdditiveModifiers();

        if (EquippedLeftHand != null)
            total += EquippedLeftHand.GetAdditiveModifiers();

        if (EquippedArmor != null)
            total += EquippedArmor.GetAdditiveModifiers();

        if (EquippedBoots != null)
            total += EquippedBoots.GetAdditiveModifiers();

        if (EquippedGloves != null)
            total += EquippedGloves.GetAdditiveModifiers();

        if (EquippedHelmet != null)
            total += EquippedHelmet.GetAdditiveModifiers();

        if (EquippedNecklace != null)
            total += EquippedNecklace.GetAdditiveModifiers();

        if (EquippedLeftRing != null)
            total += EquippedLeftRing.GetAdditiveModifiers();

        if (EquippedRightRing != null)
            total += EquippedRightRing.GetAdditiveModifiers();

        return total;
    }

    public JObject GetSaveGameJson()
    {
        JObject JO = new JObject();

        JO.Add($"{nameof(SaveGameName)}", SaveGameName);
        JO.Add($"{nameof(SaveGameType)}", SaveGameType);
        JO.Add($"{nameof(PlayerName)}", PlayerName);


        // Spawning
        JObject spawnxyz = new JObject();
        if (GameObject.FindObjectOfType<PlayerControllerV2>() is PlayerControllerV2 p)
        {
            var t = p.GetComponent<Transform>();
            spawnxyz.Add("x", t.position.x);
            spawnxyz.Add("y", t.position.y);
            spawnxyz.Add("z", t.position.z);
        }
        else
        {
            spawnxyz.Add("x", NextSpawnPosition.x);
            spawnxyz.Add("y", NextSpawnPosition.y);
            spawnxyz.Add("z", NextSpawnPosition.z);
        }

        JO.Add("NextSpawnPosition", spawnxyz);
        JO.Add($"{nameof(CurrentWorldScene)}", CurrentWorldScene);

        // Character Progress: Stats, abilities, reactions XP
        var currentS = CurrentStats.GetSaveGameJson();
        var maxS = BaseStats.GetSaveGameJson();
        JO.Add($"{nameof(CurrentStats)}", currentS);
        JO.Add($"{nameof(BaseStats)}", maxS);
        JO.Add($"{nameof(XP)}", XP);

        JArray abilitiesJson = new JArray();
        foreach (AbilityV2 a in Abilities)
        {
            var saveGameJson = a.GetSaveGameJson();
            abilitiesJson.Add(saveGameJson);
        }
        JO.Add($"{nameof(Abilities)}", abilitiesJson);

        JArray reactionsJson = new JArray();
        foreach (ReactionV2 r in reactions)
        {
            var saveGameJson = r.GetSaveGameJson();
            reactionsJson.Add(saveGameJson);
        }
        JO.Add($"{nameof(Reactions)}", reactionsJson);


        // Story progress 
        Newtonsoft.Json.Linq.JArray completedQuestsJson = new JArray();
        foreach (Quest q in CompletedQuests)
        {
            var saveGameJson = q.GetSaveGameJson();
            completedQuestsJson.Add(saveGameJson);
        }
        JO.Add($"{nameof(CompletedQuests)}", completedQuestsJson);

        JArray activeQuestsJson = new JArray();
        foreach (var active in ActiveQuests)
        {
            JObject activeQuest = new JObject();
            var savegameJsonQuest = active.Key.GetSaveGameJson();
            activeQuest.Add("SaveGameName", savegameJsonQuest.GetValue("SaveGameName"));
            activeQuest.Add("SaveGameType", savegameJsonQuest.GetValue("SaveGameType"));

            JArray completedObjectives = new JArray();
            foreach (var o in active.Value)
            {
                var savegameJsonObjective = o.GetSaveGameJson();
                completedObjectives.Add(savegameJsonObjective);
            }
            activeQuest.Add("CompletedObjectives", completedObjectives);

            activeQuestsJson.Add(activeQuest);
        }
        JO.Add("ActiveQuests", activeQuestsJson);

        // Inventory and Equipped gear
        JO.Add($"{nameof(Gold)}", Gold);

        JArray inventory = new JArray();
        foreach (Item i in Items)
        {
            var saveGameJson = i.GetSaveGameJson();
            inventory.Add(saveGameJson);
        }

        JO.Add("Inventory", inventory);


        if (EquippedRightHand is IHandHeld)
            JO.Add($"{nameof(EquippedRightHand)}", EquippedRightHand.GetSaveGameJson());
        else
            JO.Add($"{nameof(EquippedRightHand)}", "null");

        if (EquippedLeftHand is IHandHeld)
            JO.Add($"{nameof(EquippedLeftHand)}", EquippedLeftHand.GetSaveGameJson());
        else
            JO.Add($"{nameof(EquippedLeftHand)}", "null");

        if (EquippedArmor is Armor)
            JO.Add($"{nameof(EquippedArmor)}", EquippedArmor.GetSaveGameJson());
        else
            JO.Add($"{nameof(EquippedArmor)}", "null");

        if (EquippedBoots is Boots)
            JO.Add($"{nameof(EquippedBoots)}", EquippedBoots.GetSaveGameJson());
        else
            JO.Add($"{nameof(EquippedBoots)}", "null");

        if (EquippedGloves is Gloves)
            JO.Add($"{nameof(EquippedGloves)}", EquippedGloves.GetSaveGameJson());
        else
            JO.Add($"{nameof(EquippedGloves)}", "null");

        if (EquippedHelmet is Helmet)
            JO.Add($"{nameof(EquippedHelmet)}", EquippedHelmet.GetSaveGameJson());
        else
            JO.Add($"{nameof(EquippedHelmet)}", "null");

        if (EquippedNecklace is Necklace)
            JO.Add($"{nameof(EquippedNecklace)}", EquippedNecklace.GetSaveGameJson());
        else
            JO.Add($"{nameof(EquippedNecklace)}", "null");

        if (EquippedLeftRing is Ring)
            JO.Add($"{nameof(EquippedLeftRing)}", EquippedLeftRing.GetSaveGameJson());
        else
            JO.Add($"{nameof(EquippedLeftRing)}", "null");

        if (EquippedRightRing is Ring)
            JO.Add($"{nameof(EquippedRightRing)}", EquippedRightRing.GetSaveGameJson());
        else
            JO.Add($"{nameof(EquippedRightRing)}", "null");

        return JO;

    }

    public void SetFromGameSaveJson(JObject saveGameData, DataManager dataManager)
    {
        Init();
        PlayerName = (string)saveGameData.GetValue("PlayerName");
        //        Debug.Log(PlayerName);

        var spawnVecJson = (JObject)saveGameData.GetValue("NextSpawnPosition");
        var spawnVecX = (float)spawnVecJson.GetValue("x");
        var spawnVecY = (float)spawnVecJson.GetValue("y");
        var spawnVecZ = (float)spawnVecJson.GetValue("z");
        NextSpawnPosition = new Vector3(spawnVecX, spawnVecY, spawnVecZ);

        CurrentWorldScene = (string)saveGameData.GetValue("CurrentWorldScene");

        var currentStatsJson = (JObject)saveGameData.GetValue("CurrentStats");
        int hc = (int)currentStatsJson.GetValue("HealthMod");
        int sc = (int)currentStatsJson.GetValue("StaminaMod");
        int mc = (int)currentStatsJson.GetValue("ManaMod");

        int pac = (int)currentStatsJson.GetValue("PhyAtkMod");
        int pdc = (int)currentStatsJson.GetValue("PhyDefMod");
        int psc = (int)currentStatsJson.GetValue("PhySpdMod");

        int mac = (int)currentStatsJson.GetValue("MagAtkMod");
        int mdc = (int)currentStatsJson.GetValue("MagDefMod");
        int msc = (int)currentStatsJson.GetValue("MagSpdMod");
        CurrentStats = IAdditiveStats.StatModifiers.Make(hc, sc, mc, pac, pdc, psc, mac, mdc, msc);

        JObject baseStatsJson = (JObject)saveGameData.GetValue("BaseStats");
        int hm = (int)baseStatsJson.GetValue("HealthMod");
        int sm = (int)baseStatsJson.GetValue("StaminaMod");
        int mm = (int)baseStatsJson.GetValue("ManaMod");

        int pam = (int)baseStatsJson.GetValue("PhyAtkMod");
        int pdm = (int)baseStatsJson.GetValue("PhyDefMod");
        int psm = (int)baseStatsJson.GetValue("PhySpdMod");

        int mam = (int)baseStatsJson.GetValue("MagAtkMod");
        int mdm = (int)baseStatsJson.GetValue("MagDefMod");
        int msm = (int)baseStatsJson.GetValue("MagSpdMod");
        BaseStats = IAdditiveStats.StatModifiers.Make(hm, sm, mm, pam, pdm, psm, mam, mdm, msm);

        XP = (int)saveGameData.GetValue("XP");


        JArray abilitiesJson = (JArray)saveGameData.GetValue("Abilities");
        foreach (JObject jo in abilitiesJson)
        {
            string abilityName = (string)jo.GetValue("SaveGameName");
            Abilities.Add(dataManager.Search<AbilityV2>(abilityName));
        }

        JArray reactionsJson = (JArray)saveGameData.GetValue("Reactions");
        foreach (JObject jo in reactionsJson)
        {
            string reactionName = (string)jo.GetValue("SaveGameName");
            Reactions.Add(dataManager.Search<ReactionV2>(reactionName));
        }

        JArray completedQuestsJSon = (JArray)saveGameData.GetValue("CompletedQuests");
        foreach (JObject jo in completedQuestsJSon)
        {
            string completedQuestName = (string)jo.GetValue("SaveGameName");
            CompletedQuests.Add(dataManager.Search<Quest>(completedQuestName));
        }

        JArray activeQuestsJSon = (JArray)saveGameData.GetValue("ActiveQuests");
        foreach (JObject jo in activeQuestsJSon)
        {
            string activeQuestName = (string)jo.GetValue("SaveGameName");
            Quest activeQuest = dataManager.Search<Quest>(activeQuestName);

            JArray completedObjectivesJson = (JArray)jo.GetValue("CompletedObjectives");
            List<Objective> completedObjectives = new List<Objective>();
            foreach (JObject joObjective in completedObjectivesJson)
            {
                string completedObjectiveName = (string)joObjective.GetValue("SaveGameName");
                Objective objective = dataManager.Search<Objective>(completedObjectiveName);
                completedObjectives.Add(objective);
            }
            ActiveQuests.Add(activeQuest, completedObjectives);
        }

        Gold = (int)saveGameData.GetValue("Gold");

        JArray inventoryJson = (JArray)saveGameData.GetValue("Inventory");

        foreach (JObject itemJson in inventoryJson)
        {
            var item = dataManager.Search<Item>(itemJson.GetValue("SaveGameName").ToString());
            Items.Add(item);
        }



        var ERH = saveGameData.GetValue("EquippedRightHand");
        if (ERH.ToString() != "null")
        {
            var ERHType = ((JObject)ERH).GetValue("SaveGameType").ToString();

            if (ERHType == "Weapon")
            {
                EquippedRightHand = dataManager.Search<Weapon>(ERHType);
            }
            else if (ERHType == "Shield")
            {
                EquippedRightHand = dataManager.Search<Shield>(ERHType);
            }
        }

        var ELH = saveGameData.GetValue("EquippedLeftHand");
        if (ELH.ToString() != "null")
        {
            var ELHType = ((JObject)ERH).GetValue("SaveGameType").ToString();

            if (ELHType == "Weapon")
            {
                EquippedRightHand = dataManager.Search<Weapon>(ELHType);
            }
            else if (ELHType == "Shield")
            {
                EquippedRightHand = dataManager.Search<Shield>(ELHType);
            }
        }

        var EA = saveGameData.GetValue("EquippedArmor");
        if (EA.ToString() != "null")
            EquippedArmor = dataManager.Search<Armor>(((JObject)EA).GetValue("SaveGameName").ToString());

        var EB = saveGameData.GetValue("EquippedBoots");
        if (EB.ToString() != "null")
            EquippedBoots = dataManager.Search<Boots>(((JObject)EB).GetValue("SaveGameName").ToString());

        var EG = saveGameData.GetValue("EquippedGloves");
        if (EB.ToString() != "null")
            EquippedGloves = dataManager.Search<Gloves>(((JObject)EG).GetValue("SaveGameName").ToString());

        var EH = saveGameData.GetValue("EquippedHelmet");
        if (EH.ToString() != "null")
            EquippedHelmet = dataManager.Search<Helmet>(((JObject)EH).GetValue("SaveGameName").ToString());

        var EN = saveGameData.GetValue("EquippedNecklace");
        if (EN.ToString() != "null")
            EquippedNecklace = dataManager.Search<Necklace>(((JObject)EN).GetValue("SaveGameName").ToString());

        var ELR = saveGameData.GetValue("EquippedLeftRing");
        if (ELR.ToString() != "null")
            EquippedLeftRing = dataManager.Search<Ring>(((JObject)ELR).GetValue("SaveGameName").ToString());

        var ERR = saveGameData.GetValue("EquippedRightRing");
        if (ERR.ToString() != "null")
            EquippedRightRing = dataManager.Search<Ring>(((JObject)ERR).GetValue("SaveGameName").ToString());

    }

    public void SetCurrentStatsToMax()
    {
        CurrentStats = MaxStats;
    }

    public bool EquipReaction(ReactionV2 reaction, string slot)
    {
        if (EquippedReactions.ContainsValue(reaction))
        {
            return false;
        }
        UnityEngine.Assertions.Assert.IsTrue(EquippedReactions.ContainsKey(slot),
            "DataManager, PlayerData, EquipReaction: reaction equip slot does not exist");

        EquippedReactions[slot] = reaction;
        return true;
    }

}