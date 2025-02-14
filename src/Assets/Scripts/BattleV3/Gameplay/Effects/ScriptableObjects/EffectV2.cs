using System.Collections;
using System.Collections.Generic;
using BattleV3;
using UnityEngine;
using UnityEngine.Serialization;
using NaughtyAttributes;

/// <summary>
/// Hierarchy goes into an array inside the AbilityV2 class
/// EffectV2 -> ChangeStatV2
/// ChangeStatV2 -> ChangeStatDurationV2 or ChangeStatImmediateV2
/// ChangeStatImmediateV2 -> Decrease or Increase Stats (ex:DecreaseStatV2)
/// </summary>
///
public class EffectV2 : ScriptableObject
{
    public enum ETargettingType
    {
        User,   // Affects the user
        PrimaryTarget, // Affects the primary target
        AllExcludeUser,  // Affects all except the ability User
        AllExcludeUserAndPrimary, // Affects all except the ability User and Primary Target
        None,   // Does not require a target
    }
    

    public ETargettingType targettingType;
    // public PreFabFXV2 fxPrefab;

    [SerializeField] private string description;
    public string Description { get => description; set => description = value; }

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

    protected void OnEnable()
    {
        SetSaveGameData();
    }

    public virtual Newtonsoft.Json.Linq.JArray GetSaveGameJson()
    {
        Newtonsoft.Json.Linq.JObject JO = new Newtonsoft.Json.Linq.JObject();
        //JO.Add($"{nameof(Description)}", Description);
        JO.Add($"{nameof(SaveGameName)}", SaveGameName);
        JO.Add($"{nameof(SaveGameType)}", SaveGameType);

        Newtonsoft.Json.Linq.JArray JA = new Newtonsoft.Json.Linq.JArray();

        return JA;

    }


    public virtual void DoEffect(GameObject user, List<GameObject> targetList, float reactionModifier = 1)
    {

    }

    public virtual void DoEffect(GameObject user, GameObject target, float reactionModifier = 1)
    {

    }
    

    public virtual void DoEffect(Battler target, float value)
    {
        
    }
    //
    public virtual void DoEffect(Battler target, int value)
    {
        
    }


    public virtual void Print()
    {
        //Debug.Log($"EffectV2: {target} {user} {targetList} {fxPrefab}");
        Debug.Log(targettingType);
    }

}
