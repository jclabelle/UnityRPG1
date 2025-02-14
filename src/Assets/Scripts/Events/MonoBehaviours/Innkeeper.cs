using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//todo: Redo with UIToolkit and new dialogues
[RequireComponent(typeof(AudioSource))]
public class Innkeeper : Vendor, IDataManager, IWorldData
{
    private DataManager dataMngr;
    private WorldDataController worldData;
    [SerializeField] private int transactionPrice;
    [SerializeField] private string transactionPrompt;

    public DataManager DataMngr { get => dataMngr; set => dataMngr = value; }
    public WorldDataController WorldData { set => worldData = value; get => worldData; }
    public int TransactionPrice { get => transactionPrice; set => transactionPrice = value; }
    public string TransactionPrompt { get => transactionPrompt; set => transactionPrompt = value; }

    protected override void Awake()
    {
        base.Awake();
        if(string.IsNullOrEmpty(TransactionPrompt) is true)
            TransactionPrompt = "Stay the night?";
    }
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        (this as IWorldData).SetWorldData();
        (this as IDataManager).SetDataManager();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public void DoInnkeeper()
    {
        if (PlayerInventory.TryPurchase(TransactionPrice) is true)
        {
            DataMngr.Player.SetCurrentStatsToMax();
            SFX.PlayOneShot(TransactionSuccessful);
        }
        else
        {
            SFX.PlayOneShot(NotEnoughGold);
            TransactionPrompt = "Not enough gold!";
        }
    }

    


}