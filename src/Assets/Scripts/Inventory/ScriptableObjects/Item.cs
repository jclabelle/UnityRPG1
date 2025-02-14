using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Linq;

[System.Serializable]
public abstract class Item : ScriptableObject, IInventoryItem, ISavePlayerData, IVendor
{
    [SerializeField] string description;
    [SerializeField] private string saveGameName;
    [SerializeField] private string saveGameType;
    [SerializeField] private bool isSaleable;
    [SerializeField] private int sellPrice;
    [SerializeField] private int buyPrice;
    [SerializeField] private Texture2D icon;

    public string Description { get => description; set => description = value; }
    public string SaveGameName { get => saveGameName; set => saveGameName = value; }
    public string SaveGameType { get => saveGameType; set => saveGameType = value; }
    public bool IsSaleable { get => isSaleable; set => isSaleable = value; }
    public int SellPrice { get => sellPrice; set => sellPrice = value; }
    public int BuyPrice { get => buyPrice; set => buyPrice = value; }
    public Texture2D Icon { get => icon; set => icon = value; }

    public void SetSaveGameData()
    {
        SaveGameName = $"{this}".Trim();
        SaveGameType = $"{this.GetType()}".Trim();
    }

    public virtual Newtonsoft.Json.Linq.JObject GetSaveGameJson()
    {
        Newtonsoft.Json.Linq.JObject JO = new Newtonsoft.Json.Linq.JObject();
        JO.Add($"{nameof(SaveGameName)}", SaveGameName);
        JO.Add($"{nameof(SaveGameType)}", SaveGameType);

        return JO;

    }

    public string GetDisplayableName()
    {
        return this.ToString().Remove(
            this.ToString().Count() - (this.GetType().ToString().Count() + 3)
            );
    }

    public abstract string GetDisplayableStats();

    public abstract string GetDisplayableDescription();


    protected void OnEnable()
    {
        SetSaveGameData();
    }

    public virtual string GetStatsAsString()
    {
        return $"{SaveGameName}";
    }

    public Texture2D GetDisplayableIcon()
    {
        if (Icon is Texture2D)
            return Icon;
        else
            return CustomTools.GUITools.CreateBackground(0.01f, 0.01f, Color.red);
    }

}

