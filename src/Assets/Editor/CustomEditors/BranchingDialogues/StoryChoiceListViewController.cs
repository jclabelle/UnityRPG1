using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.UIElements;

public class StoryChoiceListViewController<T> where T : IDisplayableInformation
{
    // The Inventory Items to display
    List<T> m_SourceData { set; get; }

    // UXML Template for item list entries
    VisualTreeAsset m_ListEntryTemplate { set; get; }

    // UI element references
    ListView m_MenuList { set; get; }

    //// Monobehaviour
    //MenuBase Controller { get; set; }

    int ItemHeight { set; get; }

    void EnumerateAllItems(List<T> source)
    {
        m_SourceData = new List<T>();
        m_SourceData.AddRange(source);
    }

    public ListView InitializeItemList(VisualElement root, VisualTreeAsset listElementTemplate, List<T> source, Action<IEnumerable<object>> notifyController)
    {
        EnumerateAllItems(source);

        // Store references
        m_ListEntryTemplate = listElementTemplate;
        m_MenuList = root.Q<ListView>("MenuList");

        FillItemList();

        // Register to get a callback when an Item is selected in InventoryList
        m_MenuList.onSelectionChange += notifyController;

        return m_MenuList;

    }

    public ListView InitializeItemList(VisualElement root, VisualTreeAsset listElementTemplate, List<T> source, Action<IEnumerable<object>> notifyController, string listviewMenuName)
    {
        EnumerateAllItems(source);

        // Store references
        m_ListEntryTemplate = listElementTemplate;
        m_MenuList = root.Q<ListView>(listviewMenuName);

        FillItemList();

        // Register to get a callback when an Item is selected in InventoryList
        m_MenuList.onSelectionChange += notifyController;

        return m_MenuList;

    }

    void FillItemList()
    {
        // Set up a function that creates an InventoryMenuListEntry in the InventoryList
        m_MenuList.makeItem = () =>
        {
            // Instantiate the UXML template for an entry
            var newListEntry = m_ListEntryTemplate.Instantiate();

            // Instantiate a controller for the Item Data
            var newListEntryLogic = new StoryChoiceListViewEntryController<T>();

            // Assign the controller to the Visual Tree. The controller is stored inside userData,
            // making it accessible for pooling later on to assign different Items to the list element.
            newListEntry.userData = newListEntryLogic;

            // Initialize the controller's ItemName label by passing the UXML root
            newListEntryLogic.SetVisualElement(newListEntry);

            // Return the root (template containers represent the root of a UXML file)
            return newListEntry;
        };

        // Set up a function used as a callback that will bind a specific Item
        // in the PlayerInventory to an InventoryMenuListEntry in the InventoryList
        m_MenuList.bindItem = (item, index) =>
        {
            (item.userData as StoryChoiceListViewEntryController<T>).SetItemData(m_SourceData[index]);
        };

        ItemHeight = (int)(Screen.height * 0.05f);

        // Set item height
        m_MenuList.fixedItemHeight = ItemHeight;

        // Set the item's source
        m_MenuList.itemsSource = m_SourceData;


    }
}