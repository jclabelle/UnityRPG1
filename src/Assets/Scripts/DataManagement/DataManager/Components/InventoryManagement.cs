using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryManagement: IReward
{
    DataManager dataManager;
    public DataManager DataMngr { get => dataManager; set => dataManager = value; }

    public void SetDataManager(DataManager d)
    {
        DataMngr = d;
    }

    public bool TryPurchaseItem(IInventoryItem item, int price)
    {
        if (price <= DataMngr.Player.Gold)
        {
            DataMngr.Player.Gold -= price;
            AddToInventory(item);
            return true;
        }
        return false;
    }

    public bool TryPurchase(int price)
    {
        if(price <= DataMngr.Player.Gold)
        {
            DataMngr.Player.Gold -= price;
            return true;
        }
        return false;

    }

    public void AddToInventory(IInventoryItem item)
    {
        if (item is not null)
        {
            DataMngr.Player.Items.Add(item);
        }
    }

    public void AddToInventory(List<IInventoryItem> items)
    {
        if(items is not null)
        {
            DataMngr.Player.Items.AddRange(items);
        }
    }

    public List<T> GetAllOfType<T>(T itemType)
    {
        return Enumerable.OfType<T>(DataMngr.Player.Items).ToList();
    }

    public List<IInventoryItem> GetFullInventoryAsIInventoryItems()
    {
        return DataMngr.Player.Items;
    }

    public List<Item> GetFullInventoryAsItems()
    {
        var listItems = new List<Item>();
        foreach (var item in DataMngr.Player.Items)
            listItems.Add(item as Item);
        return listItems;
    }

    public List<IEquippable> GetFullEquipment()
    {
        List<IEquippable> equipment = new List<IEquippable>();

        if (DataMngr.Player.EquippedRightHand != null)
            equipment.Add(DataMngr.Player.EquippedRightHand as IEquippable);

        if (DataMngr.Player.EquippedLeftHand != null)
            equipment.Add(DataMngr.Player.EquippedLeftHand as IEquippable);

        if (DataMngr.Player.EquippedArmor != null)
            equipment.Add(DataMngr.Player.EquippedArmor);

        if (DataMngr.Player.EquippedBoots != null)
            equipment.Add(DataMngr.Player.EquippedBoots);

        if (DataMngr.Player.EquippedGloves != null)
            equipment.Add(DataMngr.Player.EquippedGloves);

        if (DataMngr.Player.EquippedHelmet != null)
            equipment.Add(DataMngr.Player.EquippedHelmet);

        if (DataMngr.Player.EquippedNecklace != null)
            equipment.Add(DataMngr.Player.EquippedNecklace);

        if (DataMngr.Player.EquippedLeftRing != null)
            equipment.Add(DataMngr.Player.EquippedLeftRing);

        if (DataMngr.Player.EquippedRightRing != null)
            equipment.Add(DataMngr.Player.EquippedRightRing);

        return equipment;
    }

    public Dictionary<string,T> GetFullEquipment<T>() where T : Equipment, IEquippable, IInventoryItem 
    {
        Dictionary<string, T> equipment = new Dictionary<string, T>();

        if (DataMngr.Player.EquippedRightHand != null)
            equipment.Add($"{nameof(DataMngr.Player.EquippedRightHand)}", DataMngr.Player.EquippedRightHand as T);

        if (DataMngr.Player.EquippedLeftHand != null)
            equipment.Add($"{nameof(DataMngr.Player.EquippedLeftHand)}", DataMngr.Player.EquippedLeftHand as T);

        if (DataMngr.Player.EquippedArmor != null)
            equipment.Add($"{nameof(DataMngr.Player.EquippedArmor)}", DataMngr.Player.EquippedArmor as T);

        if (DataMngr.Player.EquippedBoots != null)
            equipment.Add($"{nameof(DataMngr.Player.EquippedBoots)}", DataMngr.Player.EquippedBoots as T);

        if (DataMngr.Player.EquippedGloves != null)
            equipment.Add($"{nameof(DataMngr.Player.EquippedGloves)}", DataMngr.Player.EquippedGloves as T);

        if (DataMngr.Player.EquippedHelmet != null)
            equipment.Add($"{nameof(DataMngr.Player.EquippedHelmet)}", DataMngr.Player.EquippedHelmet as T);

        if (DataMngr.Player.EquippedNecklace != null)
            equipment.Add($"{nameof(DataMngr.Player.EquippedNecklace)}", DataMngr.Player.EquippedNecklace as T);

        if (DataMngr.Player.EquippedLeftRing != null)
            equipment.Add($"{nameof(DataMngr.Player.EquippedLeftRing)}", DataMngr.Player.EquippedLeftRing as T);

        if (DataMngr.Player.EquippedRightRing != null)
            equipment.Add($"{nameof(DataMngr.Player.EquippedRightRing)}", DataMngr.Player.EquippedRightRing as T);

        return equipment;
    }

    public void Equip(IEquippable equippable, bool rightHand = true)
    {
        if (equippable is not null)
        {
            // If this comes from the inventory, remove it.
            if (DataMngr.Player.Items.Contains(equippable as IInventoryItem))
            {
                DataMngr.Player.Items.Remove(equippable as IInventoryItem);
            }
        }

        // restrict to one shield
        if (equippable is Shield)
        {
            if(rightHand is true && DataMngr.Player.EquippedLeftHand is Shield)
            {
                AddToInventory((IInventoryItem)DataMngr.Player.EquippedLeftHand);
                DataMngr.Player.EquippedLeftHand = null;
            }
            else if(rightHand is false && DataMngr.Player.EquippedRightHand is Shield)
            {
                AddToInventory((IInventoryItem)DataMngr.Player.EquippedRightHand);
                DataMngr.Player.EquippedRightHand = null;
            }
        }


        if (equippable is IHandHeld heldOneH && heldOneH.IsTwoHander == false)
        {
            if(rightHand is true && DataMngr.Player.EquippedRightHand is not null)
            {
                AddToInventory(DataMngr.Player.EquippedRightHand as IInventoryItem);
                DataMngr.Player.EquippedRightHand = heldOneH;
            }
            else if (rightHand is true)
            {
                DataMngr.Player.EquippedRightHand = heldOneH;
            }
            else if(rightHand is false && DataMngr.Player.EquippedLeftHand is not null)
            {
                AddToInventory(DataMngr.Player.EquippedLeftHand as IInventoryItem);
                DataMngr.Player.EquippedLeftHand = heldOneH;
            }
            else
            {
                DataMngr.Player.EquippedLeftHand = heldOneH;
            }
        } else if(equippable is IHandHeld heldTwoH && heldTwoH.IsTwoHander == true)
        {
            AddToInventory(DataMngr.Player.EquippedRightHand as IInventoryItem);
            AddToInventory(DataMngr.Player.EquippedLeftHand as IInventoryItem);

            if(rightHand is true)
            {
                DataMngr.Player.EquippedRightHand = heldTwoH;
                DataMngr.Player.EquippedLeftHand = null;
            }
            else
            {
                DataMngr.Player.EquippedLeftHand = heldTwoH;
                DataMngr.Player.EquippedRightHand = null;
            }
        }

        if (equippable is Armor armor)
        {
            AddToInventory((IInventoryItem)DataMngr.Player.EquippedArmor);
            DataMngr.Player.EquippedArmor = armor;
        }

        if (equippable is Boots boots)
        {
            AddToInventory((IInventoryItem)DataMngr.Player.EquippedBoots);
            DataMngr.Player.EquippedBoots = boots;
        }

        if (equippable is Gloves gloves)
        {
            AddToInventory((IInventoryItem)DataMngr.Player.EquippedGloves);
            DataMngr.Player.EquippedGloves = gloves;
        }

        if (equippable is Helmet helmet)
        {
            AddToInventory((IInventoryItem)DataMngr.Player.EquippedHelmet);
            DataMngr.Player.EquippedHelmet = helmet;
        }

        if (equippable is Necklace necklace)
        {
            AddToInventory((IInventoryItem)DataMngr.Player.EquippedNecklace);
            DataMngr.Player.EquippedNecklace = necklace;
        }

        if (equippable is Ring ring)
        {
            if (rightHand is true && DataMngr.Player.EquippedRightRing is not null)
            {
                AddToInventory((IInventoryItem)DataMngr.Player.EquippedRightRing);
                DataMngr.Player.EquippedRightRing = ring;
            }
            else if (rightHand is true)
            {
                DataMngr.Player.EquippedRightRing = ring;
            }
            else if (rightHand is false && DataMngr.Player.EquippedLeftRing is not null)
            {
                AddToInventory((IInventoryItem)DataMngr.Player.EquippedLeftRing);
                DataMngr.Player.EquippedLeftRing = ring;
            }
            else
            {
                DataMngr.Player.EquippedLeftRing = ring;
            }
        }

    }

    public void UnEquip(IEquippable equippable)
    {
        // Todo: need to be redone to leverage the interface.
        // Destructive as is,does not send equipment back to the inventory.

        //if (equippable is Armor)
        //    DataMngr.Player.EquippedArmor = null;

        //if (equippable is Boots)
        //    DataMngr.Player.EquippedBoots = null;

        //if (equippable is Gloves)
        //    DataMngr.Player.EquippedGloves = null;

        //if (equippable is Helmet)
        //    DataMngr.Player.EquippedHelmet = null;

        //if (equippable is Necklace)
        //    DataMngr.Player.EquippedNecklace = null;

        //if (equippable is Ring)
        //{
        //    DataMngr.Player.EquippedLeftRing = null;
        //    DataMngr.Player.EquippedRightRing = null;
        //}
    }

    public IAdditiveStats.StatModifiers GetEquipmentModifiers()
    {
        IAdditiveStats.StatModifiers total = new IAdditiveStats.StatModifiers();

        if (DataMngr.Player.EquippedRightHand != null)
            total += DataMngr.Player.EquippedRightHand.GetAdditiveModifiers();

        if (DataMngr.Player.EquippedLeftHand != null)
            total += DataMngr.Player.EquippedLeftHand.GetAdditiveModifiers();

        if (DataMngr.Player.EquippedArmor != null)
            total += DataMngr.Player.EquippedArmor.GetAdditiveModifiers();

        if (DataMngr.Player.EquippedBoots != null)
            total += DataMngr.Player.EquippedBoots.GetAdditiveModifiers();

        if (DataMngr.Player.EquippedGloves != null)
            total += DataMngr.Player.EquippedGloves.GetAdditiveModifiers();

        if (DataMngr.Player.EquippedHelmet != null)
            total += DataMngr.Player.EquippedHelmet.GetAdditiveModifiers();

        if (DataMngr.Player.EquippedNecklace != null)
            total += DataMngr.Player.EquippedNecklace.GetAdditiveModifiers();

        if (DataMngr.Player.EquippedLeftRing != null)
            total += DataMngr.Player.EquippedLeftRing.GetAdditiveModifiers();

        if (DataMngr.Player.EquippedRightRing != null)
            total += DataMngr.Player.EquippedRightRing.GetAdditiveModifiers();

        return total;
    }



    public void Test_PrintAllOfType<T>()
    {
        var found = Enumerable.Where<IInventoryItem>(DataMngr.Player.Items, t => t is T);

        foreach (var i in found)
        {
            Debug.Log(i + " " + i.GetType());
        }
    }

    public void Test_PrintAll()
    {

        foreach (var i in DataMngr.Player.Items)
        {
            Debug.Log(i + " " + i.GetType());
        }

    }

    public string Test_GetStringAllEquipModifiers()
    {
        var all = DataMngr.Player.GetEquipmentModifiers();

        return IAdditiveStats.StatModifiers.GetString(all);

    }

    public void AddReward(IReward.Reward reward)
    {
        throw new System.NotImplementedException();
    }
}