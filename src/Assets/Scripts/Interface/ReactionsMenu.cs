using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ReactionsMenu : ListViewMenu
{

    protected override void Start()
    {
        base.Start();
        foreach (var fb in FocusableButtons)
            fb.Value.focusable = false;

        SetSlotNames();

        foreach(var button in FocusableButtons)
        {
            //button.Value.clicked += () => DataMngr.Player.EquippedReactions["SlotThree"] = MenuListView.selectedItem as ReactionV2;
            button.Value.clicked += () => TryEquipReaction(
                MenuListView.selectedItem as ReactionV2, button.Value.name);
        }

    }

    protected override void Update()
    {
        base.Update();
    }

    private void TryEquipReaction(ReactionV2 reaction, string slot)
    {
        if(DataMngr.Player.EquipReaction(reaction, slot))
        {
            //todo: Add UI Denied/Confirmed sounds.
        }
    }

    private void SetSlotNames()
    {
        int itr = 0;
        foreach(var r in DataMngr.Player.EquippedReactions)
        {
            if(r.Value is ReactionV2 rr)
            {
                FocusableButtons.ElementAt(itr).Value.text = rr.GetDisplayableName();
            }
            else
            {
                FocusableButtons.ElementAt(itr).Value.text = "Empty\nSlot";
            }
            itr++;
        }

        //if(DataMngr.Player.EquippedReactions["SlotOne"] is ReactionV2 r1)
        //    FocusableButtons["ButtonOne"].text = r1.GetFormattedName();

        //if (DataMngr.Player.EquippedReactions["SlotOne"] is ReactionV2 r2)
        //    FocusableButtons["ButtonTwo"].text = r2.GetFormattedName();

    }

    protected override void DoConfirm()
    {
        // Give focus to buttons if the player selects an item in the list
        if(MenuListView.focusController.focusedElement == MenuListView)
        {
            //Debug.Log("Starting Do Confirm: Focus is on ListView");

            foreach (var fb in FocusableButtons)
                fb.Value.focusable = true;
            Buttons["SlotOne"].Focus();
            MenuListView.focusable = false;
        }
        else // Give focus to the list if the player selects a Reaction slot
        {
            //Debug.Log("Starting Do Confirm: Focus is on Buttons");
            SetSlotNames();
            foreach (var fb in FocusableButtons)
                fb.Value.focusable = false;
            MenuListView.focusable = true;
            MenuListView.Focus();
        }
    }

    protected override void NavLeft()
    {
        Hide();
        NotifyNav(UIController.EInterface.AbilitiesMenu);
    }

    protected override void NavRight()
    {
        NotifyNav(UIController.EInterface.InGameMenu);
    }

    protected override void SetInterfaceContent()
    {
        SetInventoryList(DataMngr.Player.Reactions);
    }
}