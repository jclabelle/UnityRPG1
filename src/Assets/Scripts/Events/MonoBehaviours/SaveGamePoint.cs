using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Dialogues;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D), typeof(SpriteRenderer), typeof(AudioSource))]
public class SaveGamePoint : MonoBehaviour, IEventProbePassive, IDataManager, IWorldData
{
    public Dialogues.IDialogue DialogueBox { get ; set ; }
    [field: SerializeField] private DialogueTree SaveGameDialogue { get; set; }
    public DataManager DataMngr { get => dataMngr; set => dataMngr = value; }
    public WorldDataController WorldData { set => worldData = value; get => worldData; }

    private DataManager dataMngr;
    private WorldDataController worldData;
    private GameObject player;
    

    [field: SerializeField] public string SaveGameText { get; set; }

    bool isActive;

    private void Awake()
    {
        UnityEngine.Assertions.Assert.IsTrue(SaveGameText is not null);
    }

    private void Start()
    {
        (this as IDataManager).SetDataManager();
        (this as IWorldData).SetWorldData();
    }

    public void EventProbePassive(GameObject player)
    {
        // Prevent player spawning on top of Save Point from triggering the event.
        if (Time.timeSinceLevelLoad > 0.5f)
        {
            DialogueBox.PlayDialogue(SaveGameDialogue);
        }
    }

    public void Update()
    {
   
    }

    public void DoSaveGame()
    {
        DataMngr.SaveGame(DataMngr.ActiveSaveSlot, DataMngr.Player);
        WorldData.SaveScenePersistentData();
        WorldData.CommitWorldData();
    }

}

