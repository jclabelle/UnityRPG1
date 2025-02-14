using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class EquipmentMenu : MenuBase
{

    private class MenuEquippedItem
    {
        public VisualElement Frame { get; set; }
        public VisualElement Icon { get; set; }
        public Equipment EquippedItem { get; set; }

        public static MenuEquippedItem Make(VisualElement f, VisualElement i, Equipment e)
        {
            MenuEquippedItem mei = new MenuEquippedItem();
            mei.Frame = f;
            mei.Icon = i;
            mei.EquippedItem = e;
            return mei;
        }
    }

    Dictionary<string, MenuEquippedItem> OccupiedEquipmentSlots;
    Dictionary<string, VisualElement> AllEquipmentSlots;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

    }

    protected override void SetUIComponents()
    {

        base.SetUIComponents();
    }

    protected override void SetCallbacks()
    {
        base.SetCallbacks();
    }

    protected override void SetInterfaceContent()
    {
        OccupiedEquipmentSlots = new Dictionary<string, MenuEquippedItem>();
        var fullEquip = DataMngr.PlayerInventory.GetFullEquipment<Equipment>();

        foreach (var equip in fullEquip)
        {
            OccupiedEquipmentSlots.Add(equip.Key,
            MenuEquippedItem.Make(
                Root.Q<VisualElement>(equip.Key),
                Root.Q<VisualElement>(equip.Key + "Icon"),
                equip.Value
                ));
        }

        AllEquipmentSlots = new Dictionary<string, VisualElement>();

        AllEquipmentSlots.Add($"{nameof(DataMngr.Player.EquippedRightHand)}", Root.Q<VisualElement>("EquippedRightHand" + "Icon"));
        AllEquipmentSlots.Add($"{nameof(DataMngr.Player.EquippedLeftHand)}", Root.Q<VisualElement>("EquippedLeftHand" + "Icon"));
        AllEquipmentSlots.Add($"{nameof(DataMngr.Player.EquippedArmor)}", Root.Q<VisualElement>("EquippedArmor" + "Icon"));
        AllEquipmentSlots.Add($"{nameof(DataMngr.Player.EquippedBoots)}", Root.Q<VisualElement>("EquippedBoots" + "Icon"));
        AllEquipmentSlots.Add($"{nameof(DataMngr.Player.EquippedGloves)}", Root.Q<VisualElement>("EquippedGloves" + "Icon"));
        AllEquipmentSlots.Add($"{nameof(DataMngr.Player.EquippedHelmet)}", Root.Q<VisualElement>("EquippedHelmet" + "Icon"));
        AllEquipmentSlots.Add($"{nameof(DataMngr.Player.EquippedNecklace)}", Root.Q<VisualElement>("EquippedNecklace" + "Icon"));
        AllEquipmentSlots.Add($"{nameof(DataMngr.Player.EquippedLeftRing)}", Root.Q<VisualElement>("EquippedLeftRing" + "Icon"));
        AllEquipmentSlots.Add($"{nameof(DataMngr.Player.EquippedRightRing)}", Root.Q<VisualElement>("EquippedRightRing" + "Icon"));

        foreach(var equip in OccupiedEquipmentSlots)
        {
            if(equip.Value.EquippedItem.Icon != null)
            {
                Debug.Log($"Assigned Image for {equip.Value.EquippedItem.SaveGameName}");
                equip.Value.Icon.style.backgroundImage = equip.Value.EquippedItem.Icon;
            }
            else
            {
                if (equip.Value.Icon.childCount > 0)
                    equip.Value.Icon.Clear();

                string name = equip.Value.EquippedItem.SaveGameName;
                name = name.Replace(' ', '\n');
                Label nameLabel = new Label(name);
                nameLabel.style.fontSize = 24;
                nameLabel.style.color = Color.white;
                equip.Value.Icon.Insert(0, nameLabel);
                nameLabel.StretchToParentSize();
            }
        }

        foreach(var slot in AllEquipmentSlots)
        {
            
            if(OccupiedEquipmentSlots.ContainsKey(slot.Key) is false && slot.Value.contentContainer.childCount <= 0)
            {
                Label name = new Label("Empty");
                name.style.fontSize = 24;
                name.style.color = Color.white;
                slot.Value.Insert(0, name);
            }
        }

    }


    protected override void ResetFocus()
    {
        AllEquipmentSlots["EquippedHelmet"].Focus();
    }

    protected override void NavRight()
    {
        Hide();
        NotifyNav(UIController.EInterface.AbilitiesMenu);

    }

    protected override void NavLeft()
    {
        Hide();
        NotifyNav(UIController.EInterface.InventoryMenu);
    }

    protected override void Update()
    {
        if (Root.style.display == DisplayStyle.Flex)
            base.Update();
    }

    protected override void DoConfirm()
    {

    }
}