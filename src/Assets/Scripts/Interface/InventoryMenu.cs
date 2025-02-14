using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;


public class InventoryMenu : MenuBase, IDataManager
{
    [SerializeField] VisualTreeAsset m_listEntryTemplate;

    //public Texture2D BackgroundTexture { set; get; }
    //public UIDocument Menu { set; get; }
    //public VisualElement Root { set; get; }
    public ListView ItemsList { set; get; }
    //public List<Label> ItemsListDataSource { set; get; }
    //public DataManager DataMngr { get; set; }
    //public IGameInterface.ChangeContext NotifyClose { set; get; }
    //public Button NavButtonStatus { set; get; }
    //public Button NavButtonRight { set; get; }

    Button m_UseOrEquipButton { set; get; }
    Button m_EquipRightHandButton{ set ; get; }
    Button m_DeleteButton { set; get; }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        ItemsList.delegatesFocus = true;
        Root.style.display = UnityEngine.UIElements.DisplayStyle.None;

    }

    protected override void SetUIComponents()
    {

        base.SetUIComponents();

        ItemsList = Root.Q<ListView>("InventoryList");

        m_UseOrEquipButton = Root.Q<Button>("UseOrEquipButton");
        m_EquipRightHandButton = Root.Q<Button>("EquipRightHandButton");
        m_DeleteButton = Root.Q<Button>("DeleteButton");

    }

    protected override void SetCallbacks()
    {

        base.SetCallbacks();
        //RegisterInputs();
        m_UseOrEquipButton.clicked += EquipSelection;
        m_EquipRightHandButton.clicked += EquipSelectionRightHand;

    }

    protected override void SetInterfaceContent()
    {
        SetInventoryList();
    }

    protected override void ResetFocus()
    {
        ItemsList.Focus();
        if (ItemsList.itemsSource.Count > 0)
        {
            if (ItemsList.itemsSource.Count > 0)
            {
                ItemsList.ClearSelection();
                ItemsList.Blur();
                ItemsList.Focus();
                ItemsList.SetSelection(0);
            }
        }
    }

    protected override void NavRight()
    {
        Hide();
        NotifyNav(UIController.EInterface.EquipmentMenu);
    }
    protected override void NavLeft()
    {
        Hide();
        NotifyNav(UIController.EInterface.InGameMenu);
    }

    private void SetInventoryList()
    {
        var itemListController = new ItemListController();
        itemListController.InitializeItemList(Root, m_listEntryTemplate, DataMngr);
    }

    private void EquipSelection()
    {
        if (ItemsList.selectedItem is IEquippable equippable)
        {
            DataMngr.PlayerInventory.Equip(equippable);
            ItemsList.itemsSource = DataMngr.PlayerInventory.GetFullInventoryAsItems();

            ItemsList.Rebuild();
            SetInterfaceContent();
        }

    }

    private void EquipSelectionRightHand()
    {
        if (ItemsList.selectedItem is IEquippable equippable)
            DataMngr.PlayerInventory.Equip(equippable, true);
    }


    protected override void Update()
    {
        if (Root.style.display == UnityEngine.UIElements.DisplayStyle.Flex)
        {
            var index = ItemsList.selectedIndex;
            ItemsList.ClearSelection();
            ItemsList.Blur();
            ItemsList.Focus();
            ItemsList.SetSelection(index);

            if (GamepadConfirm.triggered)
                Debug.Log("Submit from Inventory Menu");

            base.Update();
        }


    }

    protected override void DoConfirm()
    {
        EquipSelection();
    }
}