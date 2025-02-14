using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UIElements;


// Provides data meant for in-game display such as in menus
public interface IDisplayableInformation
{
    //public string GetDisplayableName();

    public string GetDisplayableName();

    public string GetDisplayableStats();
    public string GetDisplayableDescription();
    public Texture2D GetDisplayableIcon();

    static string GetScriptableObjectDisplayableName(ScriptableObject scriptableObject)
    {
        return scriptableObject.ToString().Remove(
            scriptableObject.ToString().Count() - (scriptableObject.GetType().ToString().Count() + 3)
        );
    }
}

public interface ISavePlayerData
{
    public string SaveGameName { set; get; }
    public string SaveGameType { set; get; }
    public void SetSaveGameData();
    public Newtonsoft.Json.Linq.JObject GetSaveGameJson();
}

public interface IWorldData
{
    WorldDataController WorldData { set; get; }

    public void SetWorldData()
    {
        WorldData = GameObject.FindObjectOfType<WorldDataController>();
    }
}

public interface IDataManager
{
    public DataManager DataMngr { get; set; }

    public void SetDataManager()
    {
        DataMngr = GameObject.FindObjectOfType<DataManager>();
    }
}

public interface IGameUserInterface
{
    public delegate void ChangeContext(UIController.EInterface eInterface);
    public ChangeContext NotifyNav { set; get; }
    public UnityEngine.UIElements.UIDocument Menu { set; get; }
    public UnityEngine.UIElements.VisualElement Root { set; get; }
    

    public void Show();

    public void Hide();

    public void RegisterWithUIController()
    {
        var uiController = GameObject.FindObjectOfType<UIController>();
        Debug.Log($"Menu name: {Menu.name}");
        uiController.Register(Menu.name, this);
        NotifyNav += uiController.ChangeContext;
    }


}

public interface IInventoryItem: IDisplayableInformation
{

}

public interface IReward
{
    [System.Serializable]
    public class Reward
    {
        public List<Item> Loot { get => loot; set => loot = value; }
        public int Gold { get => gold; set => gold = value; }
        public int XP { get => xP; set => xP = value; }

        [SerializeField] private List<Item> loot;
        [SerializeField] private int gold;
        [SerializeField] private int xP;

        static public Reward operator +(Reward a, Reward b)
        {
            a.Loot.AddRange(b.Loot);
            a.Gold += b.Gold;
            a.XP += b.XP;
            return a;
        }


        public Reward(List<Item> _loot, int _gold, int _xp)
        {
            Loot = _loot;
            Gold = _gold;
            XP = _xp;
        }

        public Reward()
        {
            Loot = new List<Item>();
        }

        public string Description()
        {
            string d = System.String.Empty;
            if (Gold > 0)
                d += $"Gold:\t{Gold}\n";

            if (XP > 0)
                d += $"XP:\t{XP}\n";

            if(Loot.Count > 0)
            {
                d += "\n";
                foreach(Item i in Loot)
                {
                    d += $"{i.name}\n";
                }
            }
            Debug.Log($"Reward Description:\n{d}");
            return d;
        }

 
    }

    [System.Serializable]
    public class SpecialReward : Reward
    {
        [SerializeField] public List<Quests.Objective> QuestObjectives { get; set; }
        [SerializeField] public List<AbilityV2> Abilities { get; set; }
        [SerializeField] public List<ReactionV2> Reactions { get; set; }

        static public SpecialReward operator +(SpecialReward a, SpecialReward b)
        {
            
            a.Loot.AddRange(b.Loot);
            a.Gold += b.Gold;
            a.XP += b.XP;
            a.QuestObjectives.AddRange(b.QuestObjectives);
            a.Abilities.AddRange(b.Abilities);
            a.Reactions.AddRange(b.Reactions);

            return a;
        }

        public Reward ToReward()
        {
            return new Reward(Loot, Gold, XP);
        }

        public SpecialReward(List<Item> _loot, int _gold, int _xp, List<Quests.Objective> _questObjectives, List<AbilityV2> _abilities, List<ReactionV2> _reactions)
            : base(_loot, _gold, _xp)
        {
            QuestObjectives = _questObjectives;
            Abilities = _abilities;
            Reactions = _reactions;
        }

        public SpecialReward() { }
    }

    public void AddReward(Reward reward);

}


public interface IAdditiveStats
{
    [System.Serializable]
    public class StatModifiers
    {
        [SerializeField] private int healthMod;
        [SerializeField] private int staminaMod;
        [SerializeField] private int manaMod;

        [SerializeField] private int phyAtkMod;
        [SerializeField] private int phyDefMod;
        [SerializeField] private int phySpdMod;

        [SerializeField] private int magAtkMod;
        [SerializeField] private int magDefMod;
        [SerializeField] private int magSpdMod;

        public int Health { get => healthMod; set => healthMod = value; }
        public int Stamina { get => staminaMod; set => staminaMod = value; }
        public int Mana { get => manaMod; set => manaMod = value; }
        public int PhyAtk { get => phyAtkMod; set => phyAtkMod = value; }
        public int PhyDef { get => phyDefMod; set => phyDefMod = value; }
        public int PhySpd { get => phySpdMod; set => phySpdMod = value; }
        public int MagAtk { get => magAtkMod; set => magAtkMod = value; }
        public int MagDef { get => magDefMod; set => magDefMod = value; }
        public int MagSpd { get => magSpdMod; set => magSpdMod = value; }

        public void ClampAll(StatModifiers max)
        {
            Health = Mathf.Clamp(Health, 0, max.Health);
            Stamina = Mathf.Clamp(Stamina, 0, max.Stamina);
            Mana = Mathf.Clamp(Mana, 0, max.Mana);

            PhyAtk = Mathf.Clamp(PhyAtk, 1, max.PhyAtk);
            PhyDef = Mathf.Clamp(PhyDef, 1, max.PhyDef);
            PhySpd = Mathf.Clamp(PhySpd, 1, max.PhySpd);

            MagAtk = Mathf.Clamp(MagAtk, 1, max.MagAtk);
            MagDef = Mathf.Clamp(MagDef, 1, max.MagDef);
            MagSpd = Mathf.Clamp(MagSpd, 1, max.MagSpd);

        }

        static public StatModifiers Make(int h, int s, int m, int pa, int pd, int ps, int ma, int md, int ms)
        {
            StatModifiers mods = new StatModifiers();

            mods.Health = h;
            mods.Stamina = s;
            mods.Mana = m;

            mods.PhyAtk = pa;
            mods.PhyDef = pd;
            mods.PhySpd = ps;

            mods.MagAtk = ma;
            mods.MagDef = md;
            mods.MagSpd = ms;

            return mods;
        }

        static public StatModifiers operator +(StatModifiers a, StatModifiers b)
        {
            return Make(a.Health + b.Health, a.Stamina + b.Stamina, a.Mana + b.Mana
                , a.PhyAtk + b.PhyAtk, a.PhyDef + b.PhyDef, a.PhySpd + b.PhySpd
                , a.MagAtk + b.MagAtk, a.MagDef + b.MagDef, a.MagSpd + b.MagSpd);
        }

        static public StatModifiers operator -(StatModifiers a, StatModifiers b)
        {
            return Make(a.Health - b.Health, a.Stamina - b.Stamina, a.Mana - b.Mana
                , a.PhyAtk - b.PhyAtk, a.PhyDef - b.PhyDef, a.PhySpd - b.PhySpd
                , a.MagAtk - b.MagAtk, a.MagDef - b.MagDef, a.MagSpd - b.MagSpd);
        }

        static public string GetString(StatModifiers sm)
        {

            string modifiersText = $"{nameof(sm.Health)}: {sm.Health}\n";
            modifiersText += $"{nameof(sm.Stamina)}: {sm.Stamina}\n";
            modifiersText += $"{nameof(sm.Mana)}: {sm.Mana}\n";

            modifiersText += $"{nameof(sm.PhyAtk)}: {sm.PhyAtk}\n";
            modifiersText += $"{nameof(sm.PhyDef)}: {sm.PhyDef}\n";
            modifiersText += $"{nameof(sm.PhySpd)}: {sm.PhySpd}\n";

            modifiersText += $"{nameof(sm.MagAtk)}: {sm.MagAtk}\n";
            modifiersText += $"{nameof(sm.MagDef)}: {sm.MagDef}\n";
            modifiersText += $"{nameof(sm.MagSpd)}: {sm.MagSpd}\n";

            return modifiersText;
        }

        static public string GetStringColumns(StatModifiers sm)
        {

            string modifiersText = $"{nameof(sm.Health)}: {sm.Health}\t";
            modifiersText += $"{nameof(sm.PhyAtk)}: {sm.PhyAtk}\t";
            modifiersText += $"{nameof(sm.MagAtk)}: {sm.MagAtk}\n";
            modifiersText += $"{nameof(sm.Stamina)}: {sm.Stamina}\t";
            modifiersText += $"{nameof(sm.PhyDef)}: {sm.PhyDef}\t";
            modifiersText += $"{nameof(sm.MagDef)}: {sm.MagDef}\n";
            modifiersText += $"{nameof(sm.Mana)}: {sm.Mana}\t";
            modifiersText += $"{nameof(sm.PhySpd)}: {sm.PhySpd}\t";
            modifiersText += $"{nameof(sm.MagSpd)}: {sm.MagSpd}\n";



            return modifiersText;
        }

        public Newtonsoft.Json.Linq.JObject GetSaveGameJson()
        {
            Newtonsoft.Json.Linq.JObject JO = new Newtonsoft.Json.Linq.JObject();

            JO.Add($"{nameof(Health)}", Health);
            JO.Add($"{nameof(Stamina)}", Stamina);
            JO.Add($"{nameof(Mana)}", Mana);

            JO.Add($"{nameof(PhyAtk)}", PhyAtk);
            JO.Add($"{nameof(PhyDef)}", PhyDef);
            JO.Add($"{nameof(PhySpd)}", PhySpd);

            JO.Add($"{nameof(MagAtk)}", MagAtk);
            JO.Add($"{nameof(MagDef)}", MagDef);
            JO.Add($"{nameof(MagSpd)}", MagSpd);

            return JO;
        }

    }

    IAdditiveStats.StatModifiers Stats { set; get; }

    public StatModifiers GetAdditiveModifiers();
}

public interface IMultiplicativeStats
{

    [System.Serializable]
    public struct StatModifiers
    {
        public float healthMod;
        public float staminaMod;
        public float manaMod;

        public float phyAtkMod;
        public float phyDefMod;
        public float phySpdMod;

        public float magAtkMod;
        public float magDefMod;
        public float magSpdMod;

        static public StatModifiers Make(float h, float s, float m, float pa, float pd, float ps, float ma, float md, float ms)
        {
            StatModifiers mods = new StatModifiers();

            mods.healthMod = h;
            mods.staminaMod = s;
            mods.manaMod = m;

            mods.phyAtkMod = pa;
            mods.phyDefMod = pd;
            mods.phySpdMod = ps;

            mods.magAtkMod = ma;
            mods.magDefMod = md;
            mods.magSpdMod = ms;

            return mods;
        }

        static public StatModifiers operator +(StatModifiers a, StatModifiers b)
        {
            return Make(a.healthMod + b.healthMod, a.staminaMod + b.staminaMod, a.manaMod + b.manaMod
                , a.phyAtkMod + b.phyAtkMod, a.phyDefMod + b.phyDefMod, a.phySpdMod + b.phySpdMod
                , a.magAtkMod + b.magAtkMod, a.magDefMod + b.magDefMod, a.magSpdMod + b.magSpdMod);
        }



        static public string GetString(StatModifiers sm)
        {

            string modifiersText = $"{nameof(sm.healthMod)}:\t{sm.healthMod}\n";
            modifiersText += $"{nameof(sm.staminaMod)}:\t{sm.staminaMod}\n";
            modifiersText += $"{nameof(sm.manaMod)}:\t{sm.manaMod}\n";

            modifiersText += $"{nameof(sm.phyAtkMod)}:\t{sm.phyAtkMod}\n";
            modifiersText += $"{nameof(sm.phyDefMod)}:\t{sm.phyDefMod}\n";
            modifiersText += $"{nameof(sm.phySpdMod)}:\t{sm.phySpdMod}\n";

            modifiersText += $"{nameof(sm.magAtkMod)}:\t{sm.magAtkMod}\n";
            modifiersText += $"{nameof(sm.magDefMod)}:\t{sm.magDefMod}\n";
            modifiersText += $"{nameof(sm.magSpdMod)}:\t{sm.magSpdMod}\n";

            return modifiersText;


        }
    }

    public float HealthMod { get; }
    public float StaminaMod { get; }
    public float ManaMod { get; }

    public float PhyAtkMod { get; }
    public float PhyDefMod { get; }
    public float PhySpdMod { get; }

    public float MagAtkMod { get; }
    public float MagDefMod { get; }
    public float MagSpdMod { get; }

    public StatModifiers GetMultiplicativeModifiers();
}

public interface IDamage
{
    public int DamageMax { get; }
    public int DamageMin { get; }
    public DecreaseStatImmediateV2.EDamageType EDamageType { get;}
    public StatV2.EStatType ValueBasedOn { get; }
    public StatV2.EStatType DamagedEStat { get; }
}

public interface IDamageOverTime : IDamage
{
    public float Duration { get; }
}

public interface IAddReactions
{
    public ReactionV2[] Reactions { get; }
}

public interface IAddAbilities
{
    public AbilityV2[] Abilities { get; }
}

public interface IHandHeld : IInventoryItem, IAdditiveStats, ISavePlayerData
{
    public bool IsTwoHander { set; get; }
}

public interface IVendor
{
    public bool IsSaleable { set; get; }
    public int SellPrice { set; get; }
    public int BuyPrice { set; get; }

    public IVendor GetVendorData()
    {
        return this;
    }
}

namespace Dialogues
{
    public interface IPopupDialogue
    {
        public delegate void StoryChoiceCallback(StoryChoice choice);

        public void StartDialogue(DialogueController dialogue, bool hideAfterLastLine = true);
        public void StartDialogue(DialogueTreeBranch branch, StoryChoiceCallback setSelectedChoiceCallback);
        public void EndDialogue();
    }
}

// Triggered by the player pressing spacebar
public interface IEventProbeActive
{
    public void EventProbeLaunched(GameObject player);
}

// Triggered by the player standing over the trigger
public interface IEventProbePassive
{
    public void EventProbePassive(GameObject player);
}
public interface IMovement
{
    public bool IsMoving();
    public Vector2 PlayerFacing { get; }
    public EDirection EDirectionOfMovement { get; set; }
    [System.Serializable]
    public enum EDirection
    {
        Up,
        Down,
        Left,
        Right,
    }
}

public interface IPlayerPosition
{
    public Vector2 PlayerPosition { get; }
}

namespace BattleV3
{

    public interface IBattlerMovement
    {
        public Movement CurrentMovement { get; set; }
        public Vector2 CurrentPosition { get; }
        public float MoveSpeed { get; set; }
        public BattleSpawnPoint SpawnPoint { get; set; }
        public void MoveTo(Movement movement);
        public void SetPosition(Vector2 position);
        public bool IsMoving { get; }
    }
}