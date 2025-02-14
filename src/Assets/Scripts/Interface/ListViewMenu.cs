using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;


public abstract class ListViewMenu : MenuBase
{
    public ListView MenuListView;

    [SerializeField]  VisualTreeAsset listviewEntryTemplate;

    Label EntryStats { set; get; }
    VisualElement EntryIcon { set; get; }

    public Dictionary<string, Button> Buttons { get; set; }
    public Dictionary<string, Button> FocusableButtons { get; set; }

    protected override void Start()
    {
        base.Start();
        MenuListView.delegatesFocus = true;
        Root.style.display = UnityEngine.UIElements.DisplayStyle.None;
    }

    protected override void Update()
    {
        // Only do the logic when the interface is visible
        if (Root.style.display == UnityEngine.UIElements.DisplayStyle.Flex) 
        {
            if (MenuListView.focusController.focusedElement == MenuListView)
            {

                var index = MenuListView.selectedIndex;
                MenuListView.ClearSelection();
                MenuListView.Blur();
                MenuListView.Focus();
                MenuListView.SetSelection(index);
            }

            base.Update();
        }
    }

    protected override void SetUIComponents()
    {
        base.SetUIComponents();

        EntryStats = Root.Q<Label>("EntryStats");
        EntryIcon = Root.Q<VisualElement>("EntryIcon");

        Buttons = Root.Query<Button>().ToList().ToDictionary(b => b.name);
        FocusableButtons = new Dictionary<string, Button>();
        foreach(var b in Buttons)
        {
            if (b.Value.focusable is true)
                FocusableButtons.Add(b.Key, b.Value);
        }

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
        MenuListView.Focus();
        if (MenuListView.itemsSource.Count > 0)
        {
            MenuListView.ClearSelection();
            MenuListView.Blur();
            MenuListView.Focus();
            MenuListView.SetSelection(0);
        }
    }

    protected abstract override void DoConfirm();

    protected abstract override void NavLeft();

    protected abstract override void NavRight();

    protected void SetInventoryList<T>(List<T> itemsToDisplay) where T: IDisplayableInformation
    {
        var listviewController = new ListviewController<T>();
        MenuListView = listviewController.InitializeItemList(Root, listviewEntryTemplate, itemsToDisplay, OnItemSelected);
    }
    
    private void SetInventoryList()
    {
        var itemListController = new ItemListController();
        itemListController.InitializeItemList(Root, listviewEntryTemplate, DataMngr);
    }

    private void SetInventoryList(List<IDisplayableInformation> itemsToDisplay)
    {
            var listviewController = new ListviewController<IDisplayableInformation>();
                listviewController.InitializeItemList(Root, listviewEntryTemplate, itemsToDisplay, OnItemSelected);
    }


    public void OnItemSelected(IEnumerable<object> selectedItems)
    {
        var selectedItem = MenuListView.selectedItem as IDisplayableInformation;

        if (selectedItem is null)
        {
            // Clear existing data
            if(EntryStats?.text != null)
                EntryStats.text = "";
            
            if(EntryIcon?.style != null)
                EntryIcon.style.backgroundImage = null;
            
            return;

        }

        // Fill data
        if(EntryStats?.text != null)
            EntryStats.text = selectedItem.GetDisplayableName();
        if(EntryIcon?.style != null)
            EntryIcon.style.backgroundImage = selectedItem.GetDisplayableIcon();
    }

    protected void RefreshItemList(List<IDisplayableInformation> items)
    {
        MenuListView.itemsSource = null;
        MenuListView.Rebuild();
        SetInventoryList(items);
        MenuListView.Rebuild();
        ResetFocus();
    }


}
