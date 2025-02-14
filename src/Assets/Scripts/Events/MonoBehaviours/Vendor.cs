using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CustomTools;
using Dialogues;
using UnityEngine;

//todo: Redo this with UIToolkit and new Dialogue system.

[RequireComponent(typeof(AudioSource))]
public class Vendor : MonoBehaviour, IEventProbeActive
{
    public IDialogue DialogueBox { get ; set ; }
    public Dialogues.DialogueController GreetingDialogue { get => greetingDialogue; set => greetingDialogue = value; }
    public string GreetingText { get => greetingText; set => greetingText = value; }
    public VendorInventory Inventory { get => inventory; set => inventory = value; }
    public int CurrentStep { get => currentStep; set => currentStep = value; }
    public GameObject Player { get => player; set => player = value; }
    public bool ShowTransactionWindow { get => showTransactionWindow; set => showTransactionWindow = value; }
    public int ButtonWidth { get => buttonWidth; set => buttonWidth = value; }
    public int ButtonHeight { get => buttonHeight; set => buttonHeight = value; }
    public int ButtonSpacingY { get => buttonSpacingY; set => buttonSpacingY = value; }
    public int GUIPosX { get => GuiPosX; set => GuiPosX = value; }
    public int GUIPosY { get => GuiPosY; set => GuiPosY = value; }
    public int ButtonSpacingX { get => buttonSpacingX; set => buttonSpacingX = value; }
    public InventoryManagement PlayerInventory { get => playerInventory; set => playerInventory = value; }
    public AudioSource SFX { get => sfx; set => sfx = value; }
    public AudioClip TransactionSuccessful { get => transactionSuccessful; set => transactionSuccessful = value; }
    public AudioClip NotEnoughGold { get => notEnoughGold; set => notEnoughGold = value; }
    public GUIStyle ButtonStyle { get => buttonStyle; set => buttonStyle = value; }
    public int GuiPosX { get => guiPosX; set => guiPosX = value; }
    public int GuiPosY { get => guiPosY; set => guiPosY = value; }

    [SerializeField] private Dialogues.DialogueController greetingDialogue;
    [SerializeField] private string greetingText;
    [SerializeField] VendorInventory inventory;
    private int currentStep;
    private GameObject player;
    private InventoryManagement playerInventory;
    private AudioSource sfx;

    /// <summary>
    /// Buy/Sell Interface
    /// </summary>
    private bool showTransactionWindow = false;
    int buttonWidth = (int)(Screen.width * 0.16);
    int buttonHeight = (int)(Screen.height * 0.05);
    int buttonSpacingX = (int)(Screen.height * 0.18);
    int buttonSpacingY = (int)(Screen.height * 0.07);
    int guiPosX;
    int guiPosY;
    [SerializeField]AudioClip transactionSuccessful;
    [SerializeField]AudioClip notEnoughGold;

    GUIStyle buttonStyle;

    void IEventProbeActive.EventProbeLaunched(GameObject player)
    {
        Player = player;
        DoStep(0);
    }

    void EventStepComplete()
    {
        currentStep++;
        DoStep(currentStep);
    }

    public void DoStep(int step)
    {
        switch (step)
        {
            case 0:
                {

                    break;
                }
            case 1:
                {
                    ShowTransactionWindow = true;
  
                    break;
                }
        }
    }

    protected virtual void Awake()
    {
        UnityEngine.Assertions.Assert.IsTrue(GreetingText is not null);

        // GreetingDialogue.Text.Add(GreetingText);
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        PlayerInventory = FindObjectOfType<DataManager>().PlayerInventory;
        SFX = gameObject.GetComponent<AudioSource>();

        UnityEngine.Assertions.Assert.IsTrue(DialogueBox is not null
                                             && PlayerInventory is not null
                                             && SFX is not null);

    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && ShowTransactionWindow is true)
        {
            ShowTransactionWindow = false;
            currentStep = 0;

        }
    }

}

