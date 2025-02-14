using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using BattleV3;
using RotaryHeart.Lib.SerializableDictionaryPro;
using UnityEngine.Serialization;
using NaughtyAttributes;

/// <summary>
///  Combine effects together to create an AbilityV2.
///  
/// </summary>
[CreateAssetMenu(menuName = "Battle/AbilityV2")]
public class AbilityV2 : ScriptableObject, ISavePlayerData, IDisplayableInformation
{
    [field: SerializeField, Required()] public BattleV3.Animations.BattleSfxSequence SfxSequence { get; set; }
    public bool HasReactionWindow => BaseReactionWindowDuration > float.Epsilon;
    public string SaveGameName { get => saveGameName; set => saveGameName = value; }
    public string SaveGameType { get => saveGameType; set => saveGameType = value; }
    [field: SerializeField] public Texture2D Icon { get; set; }
    [field: SerializeField] public string Description { get; set; }
    [field: SerializeField, MinValue(0)] public int ResourceCost { get; set; } // How much it costs to use this:
    public StatV2.EResourceType Resource { get; set; } = StatV2.EResourceType.Stamina; 
    [field: SerializeField, MinValue(0)] public float BaseReactionWindowDuration { get; set; } = 1.0f;
    [field: SerializeField, MinValue(0)] public float BaseHitSuccessProbability { get; set; } = 1.0f;
    [field:SerializeField] public EffectValueDict EffectPowerDict { get; set; }
    [field: SerializeField] public Movement.EType MovementType { get; set; } = Movement.EType.ShortStepTowardsTarget;


    /// <summary>
    /// Save Game
    /// </summary>
    private string saveGameName;
    private string saveGameType;

    public void SetSaveGameData()
    {
        SaveGameName = $"{this}".Trim();
        SaveGameType = $"{this.GetType()}".Trim();
    }

    protected void OnEnable()
    {
        SetSaveGameData();
    }

    public virtual Newtonsoft.Json.Linq.JObject GetSaveGameJson()
    {
        Newtonsoft.Json.Linq.JObject JO = new Newtonsoft.Json.Linq.JObject();
        //JO.Add($"{nameof(Description)}", Description);
        JO.Add($"{nameof(SaveGameName)}", SaveGameName);
        JO.Add($"{nameof(SaveGameType)}", SaveGameType);
        return JO;

    }

    public bool RequiresSingleEnemyTarget()
    {
        foreach (EffectV2 effect in EffectPowerDict.Keys)
        {
            if (effect.targettingType == EffectV2.ETargettingType.PrimaryTarget)
                return true;
        }
        return false;
    }

    public bool UserCanAfford(BattleV3.BattlerStats userStats)
    {
        switch (Resource)
        {
            case StatV2.EResourceType.Stamina:
                {
                    return userStats.Stamina.CurrentValue >= ResourceCost;
                }
            case StatV2.EResourceType.Mana:
                {
                    return userStats.Mana.CurrentValue >= ResourceCost;
                }
            case StatV2.EResourceType.Health:
                {
                    return userStats.Health.CurrentValue >= ResourceCost;
                }
        }

        return false;
    }

    public string GetDisplayableName()
    {
        return this.ToString().Remove(
            this.ToString().Count() - (this.GetType().ToString().Count() + 3)
            );
    }

    public string GetDisplayableStats()
    {
        throw new System.NotImplementedException();
    }

    public string GetDisplayableDescription()
    {
        throw new System.NotImplementedException();
    }

    public Texture2D GetDisplayableIcon()
    {
        return Icon ? Icon :CustomTools.GUITools.CreateBackground(0.01f, 0.01f, Color.red);
    }

    public bool HasEffectOfTargetType(EffectV2.ETargettingType eTargettingTypeType)
    {
        return EffectPowerDict.Keys.Any(effect => effect.targettingType == eTargettingTypeType);
    }

    [System.Serializable]
    public class EffectValueDict : SerializableDictionary<EffectV2, float> {}

   
}
