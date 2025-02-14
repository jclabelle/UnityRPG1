using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class ItemListController 
{
    // The Inventory Items to display
    List<Item> m_PlayerInventory { set; get; }

    // UXML Template for item list entries
    VisualTreeAsset m_ListEntryTemplate { set; get; }

    // UI element references
    ListView m_InventoryList { set; get; }
    Label m_ItemStatsLabel { set; get; }
    VisualElement m_ItemIcon { set; get; }
    Button m_UseOrEquipButton { set; get; }
    Button m_EquipRightHandButton { set; get; }
    Button m_DeleteButton { set; get; }


    int ItemHeight { set; get; }

    void EnumerateAllItems(DataManager DataMngr)
    {
        m_PlayerInventory = new List<Item>();
        m_PlayerInventory.AddRange(DataMngr.PlayerInventory.GetFullInventoryAsItems());
    }

    public void InitializeItemList(VisualElement root, VisualTreeAsset listElementTemplate, DataManager DataMngr)
    {
        EnumerateAllItems(DataMngr);

        // Store references
        m_ListEntryTemplate = listElementTemplate;
        m_InventoryList = root.Q<ListView>("InventoryList");
        m_ItemStatsLabel = root.Q<Label>("ItemStats");
        m_ItemIcon = root.Q<VisualElement>("ItemIcon");
        m_UseOrEquipButton = root.Q<Button>("UseOrEquipButton");
        m_EquipRightHandButton = root.Q<Button>("EquipRightHandButton");
        m_DeleteButton = root.Q<Button>("DeleteButton");

        FillItemList();

        // Register to get a callback when an Item is selected in InventoryList
        m_InventoryList.onSelectionChange += OnItemSelected;
    }

    void OnItemSelected(IEnumerable<object> selectedItems)
    {
        // Get the currently selected Item directly from the InventoryList
        var selectedItem = m_InventoryList.selectedItem as Item;

        // If nothing is selected at the moment
        if(selectedItem is null)
        {
            // Clear existing data
            m_ItemStatsLabel.text = "";
            m_ItemIcon.style.backgroundImage = null;

            // Disable buttons
            m_EquipRightHandButton.SetEnabled(false);
            m_UseOrEquipButton.SetEnabled(false);
            m_DeleteButton.SetEnabled(false);

            return;
        }

        // Fill in data
        m_ItemStatsLabel.text = selectedItem is null ? string.Empty :  selectedItem.GetStatsAsString();
        m_ItemIcon.style.backgroundImage = selectedItem is null ? null : new StyleBackground(selectedItem.GetDisplayableIcon());

        // Enable Buttons
        m_UseOrEquipButton.SetEnabled(true);
        m_DeleteButton.SetEnabled(true);

        // Set Use or Equip && contextual RH button
        if (selectedItem is IEquippable)
        {
            if(selectedItem is Weapon w)
            {
                if(w.IsTwoHander is true)
                {
                    //m_EquipRightHandButton.style.opacity = 0;
                    m_EquipRightHandButton.SetEnabled(false);
                    m_UseOrEquipButton.text = "Equip (2H)";
                }
                else
                {
                    m_EquipRightHandButton.SetEnabled(true);
                    //m_EquipRightHandButton.style.opacity = 1;
                    m_UseOrEquipButton.text = "Equip (L)";
                }
            }
            else
            {
                m_UseOrEquipButton.text = "Equip";
                //m_EquipRightHandButton.style.opacity = 0;
                m_EquipRightHandButton.SetEnabled(false);
            }
        }
        else
            m_UseOrEquipButton.text = "Use";
    }

    void FillItemList()
    {
        // Set up a function that creates an InventoryMenuListEntry in the InventoryList
        m_InventoryList.makeItem = () =>
        {
            // Instantiate the UXML template for an entry
            var newListEntry = m_ListEntryTemplate.Instantiate();

            // Instantiate a controller for the Item Data
            var newListEntryLogic = new ItemListEntryController();

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
        m_InventoryList.bindItem = (item, index) =>
        {
            (item.userData as ItemListEntryController).SetItemData(m_PlayerInventory[index]);
        };

        ItemHeight = (int)(Screen.height * 0.05f);

        // Set item height
        m_InventoryList.fixedItemHeight = ItemHeight;

        // Set the item's source
        m_InventoryList.itemsSource = m_PlayerInventory;


    }

}