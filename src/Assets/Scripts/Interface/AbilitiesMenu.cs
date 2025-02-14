using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;

public class AbilitiesMenu : MenuBase
{
    public ListView m_MenuList;

    [SerializeField] VisualTreeAsset m_listviewEntryTemplate;

    Label m_EntryStats { set; get; }
    VisualElement m_EntryIcon { set; get; }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        m_MenuList.delegatesFocus = true;
        Root.style.display = UnityEngine.UIElements.DisplayStyle.None;
    }

    protected override void SetUIComponents()
    {
        base.SetUIComponents();

        m_EntryStats = Root.Q<Label>("EntryStats");
        m_EntryIcon = Root.Q<VisualElement>("EntryIcon");
    }

    protected override void SetCallbacks()
    {
        base.SetCallbacks();
    }

    protected override void SetInterfaceContent()
    {
        SetInventoryList();
    }

    protected override void ResetFocus()
    {
        m_MenuList.Focus();
        if(m_MenuList.itemsSource.Count > 0)
        {
            m_MenuList.ClearSelection();
            m_MenuList.Blur();
            m_MenuList.Focus();
            m_MenuList.SetSelection(0);
        }
    }

    protected override void NavRight()
    {
        Hide();
        NotifyNav(UIController.EInterface.ReactionsMenu);
    }

    protected override void NavLeft()
    {
        Hide();
        NotifyNav(UIController.EInterface.EquipmentMenu);
    }

    private void SetInventoryList()
    {
        var listviewController = new ListviewController<AbilityV2>();
        m_MenuList = listviewController.InitializeItemList(Root, m_listviewEntryTemplate, DataMngr.Player.Abilities, OnItemSelected);
    }

    public void OnItemSelected(IEnumerable<object> selectedItems)
    {
        var selectedItem = m_MenuList.selectedItem as IDisplayableInformation;

        //m_MenuList.Focus();

        if (selectedItem is null)
        {
            // Clear existing data
            m_EntryStats.text = "";
            m_EntryIcon.style.backgroundImage = null;
            return;
        }

        // Fill data
        m_EntryStats.text = selectedItem.GetDisplayableName();
        m_EntryIcon.style.backgroundImage = selectedItem.GetDisplayableIcon();
    }

    protected override void Update()
    {
        if (Root.style.display == UnityEngine.UIElements.DisplayStyle.Flex)
        { 
            var index = m_MenuList.selectedIndex;
            m_MenuList.ClearSelection();
            m_MenuList.Blur();
            m_MenuList.Focus();
            m_MenuList.SetSelection(index);
                
            if (GamepadConfirm.triggered)
                Debug.Log("Submit from Abilities Menu");

            base.Update();
        }
    }

    protected override void DoConfirm()
    {

    }
}