using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;


public class InGameMenu : MenuBase, IDataManager
{
    //UIDocument menu;
    //VisualElement root;
    VisualElement progress;
    Label levelValue;
    Label xpValue;
    Label goldValue;
    Label stats;
    Label equipment;
    //Button buttonInventory;
    DataManager dataMngr;
    VisualElement status;
    //UIController uiControl;

    //public UIDocument Menu { set; get; }
    //public VisualElement Root { set; get; }
    public Label LevelValue { get => levelValue; set => levelValue = value; }
    public Label XpValue { get => xpValue; set => xpValue = value; }
    public Label GoldValue { get => goldValue; set => goldValue = value; }
    public Label Stats { get => stats; set => stats = value; }
    public VisualElement Status { get => status; set => status = value; }
    public VisualElement Progress { get => progress; set => progress = value; }
    //public DataManager DataMngr { get => dataMngr; set => dataMngr = value; }
    public Label Equipment { get => equipment; set => equipment = value; }
    //public Button ButtonInventory { get => buttonInventory; set => buttonInventory = value; }
    //public IGameInterface.ChangeContext NotifyNav { set; get; }



    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        //(this as IDataManager).SetDataManager();
        //Init();
        //(this as IGameInterface).RegisterWithUIController();
        //(this as IGameInterface).Hide();

    }

    protected override void Update()
    {
        if(Root.style.display == DisplayStyle.Flex)
            base.Update();
    }


    protected override void SetUIComponents()
    {
        base.SetUIComponents();
        //Menu = GetComponent<UIDocument>();
        //Root = Menu.rootVisualElement;

        Progress = Root.Q<VisualElement>("Background").Q<VisualElement>("Menu").Q<VisualElement>("Character").Q<VisualElement>("Progress");
        Status = Root.Q<VisualElement>("Background").Q<VisualElement>("Menu").Q<VisualElement>("Status");

        Stats = Status.Q<Label>("Stats");
        Equipment = Status.Q<Label>("Equipment");
        //ButtonInventory = Root.Q<Button>("ButtonInventory");


        LevelValue = Progress.Q<VisualElement>("Level").Q<Label>("Level_Value");
        XpValue = Progress.Q<VisualElement>("XP").Q<Label>("XP_Value");
        GoldValue = Progress.Q<VisualElement>("Gold").Q<Label>("Gold_Value");
    }

    protected override void SetCallbacks()
    {
        base.SetCallbacks();
        //ButtonInventory.clicked += OpenInventory;
    }

    protected override void SetInterfaceContent()
    {
        LevelValue.text = "Not Impl";
        XpValue.text = DataMngr.Player.XP.ToString();
        GoldValue.text = DataMngr.Player.Gold.ToString();

        string statsText = System.String.Empty;
        statsText += $"Health:\t{DataMngr.Player.CurrentStats.Health}/{DataMngr.Player.MaxStats.Health }\n";
        statsText += $"Stamina:\t{DataMngr.Player.CurrentStats.Stamina}/{DataMngr.Player.MaxStats.Stamina }\n";
        statsText += $"Mana:\t\t{DataMngr.Player.CurrentStats.Mana}/{DataMngr.Player.MaxStats.Mana }\n";
        statsText += $"PhyAtk:\t{DataMngr.Player.CurrentStats.PhyAtk }/{DataMngr.Player.MaxStats.PhyAtk }\n";
        statsText += $"PhyDef:\t{DataMngr.Player.CurrentStats.PhyDef }/{DataMngr.Player.MaxStats.PhyDef }\n";
        statsText += $"PhySpd:\t{DataMngr.Player.CurrentStats.PhySpd }/{DataMngr.Player.MaxStats.PhySpd }\n";
        statsText += $"MagAtk:\t{DataMngr.Player.CurrentStats.MagAtk }/{DataMngr.Player.MaxStats.MagAtk }\n";
        statsText += $"MagDef:\t{DataMngr.Player.CurrentStats.MagDef }/{DataMngr.Player.MaxStats.MagDef }\n";
        statsText += $"MagSpd:\t{DataMngr.Player.CurrentStats.MagSpd }/{DataMngr.Player.MaxStats.MagSpd }\n";

        Stats.text = statsText;


        var fullEquipment = DataMngr.PlayerInventory.GetFullEquipment();

        if (fullEquipment is List<IEquippable> fe)
        {
            string equipList = string.Empty;
            foreach (var equip in fe)
            {
                equipList += equip.ToString() + "\n";
            }

            Equipment.text = equipList;
        }

    }

    protected override void ResetFocus()
    {

    }


    protected override void NavRight()
    {
        Hide();
        NotifyNav(UIController.EInterface.InventoryMenu);
    }

    protected override void NavLeft()
    {
        Hide();
        NotifyNav(UIController.EInterface.ReactionsMenu);

    }

    protected override void DoConfirm()
    {

    }
}
