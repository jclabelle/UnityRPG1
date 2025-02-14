using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using Dialogues;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

[RequireComponent(typeof(BoxCollider2D), typeof(SpriteRenderer), typeof(AudioSource))]
public class TreasureChest : PersistentMonobehaviour, IEventProbeActive
{
    public IDialogue DialogueBox { get => dialogueBox; set => dialogueBox = value; }
    public IReward.Reward Treasure { get => treasure; set => treasure = value; }
    public bool IsLooted { get => isLooted; set => isLooted = value; }
    public Sprite SpriteOpen { get => spriteOpen; set => spriteOpen = value; }
    public Sprite SpriteClosed { get => spriteClosed; set => spriteClosed = value; }
    public AudioClip OpenSound { get => openSound; set => openSound = value; }

    [SerializeField] private IReward.Reward treasure;
    [SerializeField] private Sprite spriteOpen;
    [SerializeField] private Sprite spriteClosed;
    [SerializeField] private AudioClip openSound;
    private IDialogue dialogueBox;
    [SerializeField] private bool isLooted;
    
    IPopupDialogue popupDialogue;


    void IEventProbeActive.EventProbeLaunched(GameObject player)
    {
        if (IsLooted is false)
        {
            var dataMngr = FindObjectOfType<DataManager>();
            gameObject.GetComponent<AudioSource>().PlayOneShot(openSound);
            IsLooted = true;
            gameObject.GetComponent<SpriteRenderer>().sprite = SpriteOpen;
            var reward = GetReward();
            dataMngr.PlayerRewards.Add(reward);
            var desc = Treasure.Description();
            var dialogue = DialogueController.InstantiateAnonOneLiner(desc);
            popupDialogue.StartDialogue(dialogue, true);
        }
    }
    private void OnDestroy()
    {
        //Debug.Log("Treasure Chest Destroyed: " + this.GetInstanceID());
    }

    public IReward.Reward GetReward()
    {
        return Treasure;
    }

    private void Awake()
    {
        //Debug.Log("Treasure Chest Awake: " + this.GetInstanceID());

        UnityEngine.Assertions.Assert.IsTrue(SpriteOpen is not null
                                            && SpriteClosed is not null
                                            && Treasure is not null);
    }

    private void Update()
    {
        //Debug.Log("Update: " + this.GetInstanceID());

    }

    private void Start()
    {
        // (this as IDialogueWindow).SetDialogueWindow();
        popupDialogue = FindObjectOfType<PopupDialogue>().GetComponent<PopupDialogue>();

        if(IsLooted is false)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = spriteClosed;
        }

        //UnityEngine.Assertions.Assert.IsTrue(DialogueBox is not null);

    }

    public override Dictionary<string, string> GetPersistentData()
    {
        Dictionary<string, string> pData = new Dictionary<string, string>();

        pData.Add($"{nameof(IsLooted)}", Convert.ToString(IsLooted));

        return pData;


    }
    public override void SetPersistentData(Dictionary<string, string> pData)
    {
        IsLooted = Convert.ToBoolean(pData[nameof(IsLooted)]);

        if (IsLooted is true)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = SpriteOpen;
        }
    }

}