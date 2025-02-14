using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BattleV3;
using UnityEngine;
using UnityEngine.InputSystem;


public class BattleSelectMenu : ListViewMenu
{
    [field: SerializeField] public List<BattlePlayerMenuAction> Actions { get; set; }

    private State internalState = State.SelectAction;
    private BattlePlayerMenuAction LastHighLevelAction { get; set; }
    public (List<Battler>, AbilityV2) BattleAction { get; set; }
    public bool IsFinalized => internalState == State.SelectionFinalized;
    public InputAction GamepadCancel { set; get; }
    [SerializeField] private float MenuX;
    [SerializeField] private float MenuY;
    [SerializeField] private float MenuZ;
    
    protected override void Start()
    {
        
        base.Start();
        GamepadCancel = new InputAction(type: InputActionType.Button, binding: "<Gamepad>/buttonSouth", interactions: "press(behavior=1)");
        GamepadCancel.Enable();
        BattleAction = (null, null);
        
        Root.transform.position = new Vector3(
        
            MenuX,
            MenuY,
            MenuZ
        );

    }

    protected override void Update()
    {
        if (Root.style.display == UnityEngine.UIElements.DisplayStyle.Flex)
        {
            Root.transform.position = new Vector3(
        
                MenuX,
                MenuY,
                MenuZ
            );
            
            base.Update();
            if (GamepadCancel.triggered)
                DoCancel();
        }
    }

    private void DoCancel()
    {
        if (internalState is State.SelectAbility or State.SelectItem)
        {
            SwitchStateSelectAction();
            return;
        }

        if (internalState is State.SelectTarget && LastHighLevelAction.name == "Attack")
        {
            SwitchStateSelectAction();
            return;
        }
        
        if(internalState is State.SelectTarget && LastHighLevelAction.name == "Use Ability")
        {
            SwitchStateSelectAbility();
            return;
        }
        
        if(internalState is State.SelectTarget && LastHighLevelAction.name == "Use Item")
        {
            SwitchStateSelectItem();
            return;
        }
    }

    private enum State
    {
        SelectAction,
        SelectAbility,
        SelectItem,
        SelectTarget,
        SelectionFinalized,
    }
    
    protected override void SetInterfaceContent()
    {
        var actionsList = new List<IDisplayableInformation>(Actions.Count);
        actionsList.AddRange(Actions.Cast<IDisplayableInformation>());

        SetInventoryList(actionsList);
    }

    protected override void DoConfirm()
    {
        if (internalState is State.SelectAction)
        {
            BattleAction = (null, null);
            LastHighLevelAction = MenuListView.selectedItem as BattlePlayerMenuAction;
            switch (LastHighLevelAction.Name)
            {
                case "Attack":
                {
                    BattleAction = (null, DataMngr.Player.BasicAttack);
                    SwitchStateSelectTarget();
                    break;
                }
                case "Use Ability":
                {
                    SwitchStateSelectAbility();
                    break;
                }
                case "Use Item":
                {
                    SwitchStateSelectItem();
                    break;
                }
                case "Escape":
                {
                    SwitchStateTryEscape();
                    break;
                }
            }

            return;
        }

        if (internalState is State.SelectAbility)
        {
            BattleAction = (null, MenuListView.selectedItem as AbilityV2);

            if (BattleAction.Item2.HasEffectOfTargetType(EffectV2.ETargettingType.PrimaryTarget))
            {
                SwitchStateSelectTarget();
                return;
            }
            
            if(BattleAction.Item2.HasEffectOfTargetType(EffectV2.ETargettingType.AllExcludeUser))
                BattleAction = (Battle.GetBattlers().Where(target => !target.CompareTag("Player")).ToList(), MenuListView.selectedItem as AbilityV2);
            else if(BattleAction.Item2.HasEffectOfTargetType(EffectV2.ETargettingType.User))
                BattleAction = (Battle.GetBattlers().Where(target => target.CompareTag("Player")).ToList(), MenuListView.selectedItem as AbilityV2);
            else if (BattleAction.Item2.HasEffectOfTargetType(EffectV2.ETargettingType.None))
                throw new NotImplementedException();

            SwitchStateSelectionFinalized();
            return;
        }
        
        if (internalState is State.SelectItem)
        {
            throw new NotImplementedException();
        }

        if (internalState is State.SelectTarget)
        {
            BattleAction = (new List<Battler>(1){MenuListView.selectedItem as Battler}, BattleAction.Item2);
            SwitchStateSelectionFinalized();
            return;
        }

        if (internalState is State.SelectionFinalized)
        {
            

        }
        
    }

    private void SwitchStateSelectionFinalized()
    {
        Battle.GetBattleUI().SetPlayerAction(BattleAction);
        BattleAction = (null, null);
        Hide();
        NotifyNav(UIController.EInterface.BattleSelectReaction);
        SwitchStateSelectAction();

    }

    private void SwitchStateTryEscape()
    {
        throw new NotImplementedException();
    }

    private void SwitchStateSelectAbility()
    {
        // var items = new List<IDisplayableInformation>(DataMngr.Player.Abilities);
        
        var abilities = DataMngr.Player.Abilities;
        var abilitiesList = new List<IDisplayableInformation>(abilities.Count);
        abilitiesList.AddRange(abilities.Cast<IDisplayableInformation>());
        // MenuListView.itemsSource = abilitiesList;
        
        // MenuListView.itemsSource = null;
        // MenuListView.Rebuild();
        // SetInventoryList(abilitiesList);
        // MenuListView.Rebuild();
        

        RefreshItemList(abilitiesList);
        MenuListView.delegatesFocus = true;

        internalState = State.SelectAbility;
    }

    private void SwitchStateSelectAction()
    {
        BattleAction = (null, null);
        
        var actionsList = new List<IDisplayableInformation>(Actions.Count);
        actionsList.AddRange(Actions.Cast<IDisplayableInformation>());
        
        // MenuListView.itemsSource = null;
        // MenuListView.Rebuild();
        // SetInventoryList(actionsList);
        // MenuListView.Rebuild();
        RefreshItemList(actionsList);
        MenuListView.delegatesFocus = true;

        // SetInterfaceContent();
        internalState = State.SelectAction;

    }
    
    private void SwitchStateSelectItem()
    {
        var items = new List<IDisplayableInformation>(DataMngr.PlayerInventory.GetFullInventoryAsItems());
        RefreshItemList(items);
        internalState = State.SelectItem;
    }

    private void SwitchStateSelectTarget()
    {
        
        var potentialTargets = Battle.GetBattlers();
        var targetsList = new List<IDisplayableInformation>(potentialTargets.Count-1);
        targetsList.AddRange(potentialTargets.Where(target => !target.CompareTag("Player")).Cast<IDisplayableInformation>());
        // MenuListView.itemsSource = targetsList;
        RefreshItemList(targetsList);
        internalState = State.SelectTarget;
    }
    
    protected override void NavLeft()
    {
        
    }

    protected override void NavRight()
    {
        
    }

}