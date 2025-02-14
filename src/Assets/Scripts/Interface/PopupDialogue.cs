using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using Dialogues;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;

public class PopupDialogue : MonoBehaviour, IGameUserInterface, IPopupDialogue
{
    public IGameUserInterface.ChangeContext NotifyNav { set; get; }
    public UIDocument Menu { set; get; }
    public VisualElement Root { set; get; }
    public VisualElement SpeakerNameFrame { set; get; }
    protected Label SpeakerName { set; get; }
    protected Label DialogueText { set; get; }
    public ListView ChoicesSelection { get; set; }
    public VisualTreeAsset ChoicesEntryTemplate { get; set; }


    protected DialogueController CurrentDialogue { set; get; }
    protected bool HideAfterLastLine { set; get; }
    protected List<StoryChoice> CurrentDialogueChoices { set; get; }
    //bool IsWaitingForPlayerChoice { get; set; }

    public IPopupDialogue.StoryChoiceCallback SendBackSelectedChoice;
    public InputAction GamepadConfirm { set; get; }



    //protected delegate void NotifyDialogueIsComplete();

    string PATH_TO_POPUPMENU_VISUALTREEASSET = "VisualTreeAssets/PopupMenu";
    string PATH_TO_CHOICES_ENTRY_TEMPLATE_VISUALTREEASSET = "VisualTreeAssets/ListviewEntryTemplate";


    private void Awake()
    {

    }

    protected void Start()
    {
        //(this as IDataManager).SetDataManager();
        Init();
        (this as IGameUserInterface).RegisterWithUIController();
        Hide();
    }

    protected void Update()
    {
        // Only do the logic when the interface is visible
        if (Root.style.display == UnityEngine.UIElements.DisplayStyle.Flex)
        {
            if (ChoicesSelection.focusController.focusedElement == ChoicesSelection)
            {
                var index = ChoicesSelection.selectedIndex;
                ChoicesSelection.ClearSelection();
                ChoicesSelection.Blur();
                ChoicesSelection.Focus();
                ChoicesSelection.SetSelection(index);

                // if (GamepadConfirm.triggered)
                //     DoConfirm();
            }


        }
    }

    private void Init()
    {
        InitializeAndSetUIComponents();
    }

    protected virtual void InitializeAndSetUIComponents()
    {
        if ( (Menu = GetComponent<UIDocument>()) is null)
        {
            Menu.visualTreeAsset = Resources.Load<VisualTreeAsset>(PATH_TO_POPUPMENU_VISUALTREEASSET);
            ChoicesEntryTemplate = Resources.Load<VisualTreeAsset>(PATH_TO_CHOICES_ENTRY_TEMPLATE_VISUALTREEASSET);
        }
        // else
        // {
        //     ChoicesEntryTemplate = Resources.Load<VisualTreeAsset>(PATH_TO_CHOICES_ENTRY_TEMPLATE_VISUALTREEASSET);
        // }
        
        Root = Menu.rootVisualElement;

        SpeakerNameFrame = Root.Q<VisualElement>(nameof(SpeakerNameFrame));
        SpeakerName = Root.Q<Label>(nameof(SpeakerName));
        DialogueText = Root.Q<Label>(nameof(DialogueText));
        ChoicesSelection = Root.Q<ListView>(nameof(ChoicesSelection));

        ChoicesSelection.delegatesFocus = true;

        SpeakerName.style.display = DisplayStyle.Flex;
        SpeakerNameFrame.style.display = DisplayStyle.Flex;
        ChoicesSelection.style.display = DisplayStyle.None;

        GamepadConfirm = new InputAction(type: InputActionType.Button, binding: "<Gamepad>/buttonEast", interactions: "press(behavior=1)");
        GamepadConfirm.Enable();
    }

    public void Hide()
    {
        Root.style.display = UnityEngine.UIElements.DisplayStyle.None;
        Root.SendToBack();
    }

    public void Show()
    {
        Root.style.display = UnityEngine.UIElements.DisplayStyle.Flex;
        Root.BringToFront();
    }

    protected void SetDialogueText(string text)
    {
        DialogueText.text = text;
    }

    protected void SetDialogueSpeaker(string speakerName)
    {
        SpeakerName.text = speakerName;
    }

    protected void HideDialogueSpeaker()
    {
        SpeakerNameFrame.style.display = DisplayStyle.None;
        SpeakerName.style.display = DisplayStyle.None;
    }

    protected void ShowDialogueSpeaker()
    {
        SpeakerNameFrame.style.display = DisplayStyle.Flex;
        SpeakerName.style.display = DisplayStyle.Flex;
    }

    protected void SetDialogue(DialogueController dialogue)
    {
        CurrentDialogue = dialogue;
        CurrentDialogue.ResetProgress();
    }

    public void StartDialogue(DialogueController dialogue, bool hideAfterLastLine = true)
    {
        SendBackSelectedChoice = null;

        HideAfterLastLine = hideAfterLastLine;

        // Notify the UI Controller a Popup is starting
        Show();

        NotifyNav(UIController.EInterface.PopupMenu);

        SetDialogue(dialogue);

        ProgressDialogue();

    }

    // public void StartDialogue(DialogueTreeBranch branch, IPopupDialogue.StoryChoiceCallback setSelectedChoiceCallback)
    // {
    //     throw new NotImplementedException();
    // }

    public void StartDialogue(DialogueTreeBranch branch, IPopupDialogue.StoryChoiceCallback setSelectedChoiceCallback)
    {
       Debug.Log("Called StartDialogue, DialogueTreeBranch overload");
       // StartDialogue(branch.Dialogue.ToDialogueV2(), false);
       StartDialogue(branch.DialogueComponent.ToDialogueV2(), branch.HasSplits is false);
        CurrentDialogueChoices = branch.Choices.ToList();
        SendBackSelectedChoice = setSelectedChoiceCallback;
        SetInventoryList<StoryChoice>(CurrentDialogueChoices);
    }

    public void EndDialogue()
    {
        ClosePopup();
    }

    public void InputSubmit(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started)
            ProgressDialogue();
    }

    protected void ProgressDialogue()
    {
        Debug.Log("Progress Dialogue Called");
        if(CurrentDialogue.IsDone is false)
        {
            Debug.Log("Branch: if(CurrentDialogue.IsDone is false) ");

            var nextLine = CurrentDialogue.GetNextLine();
            
            SetDialogueText(nextLine.Text);
            if (nextLine.Type == Line.EType.Conversation)
            {
                SetDialogueSpeaker(nextLine.Speaker);
                ShowDialogueSpeaker();
            }
            else
                HideDialogueSpeaker();
        }
        else if (CurrentDialogue.IsDone is true && CurrentDialogueChoices is { Count: > 0 })
        {
            Debug.Log("Branch: else if (CurrentDialogue.IsDone is true && CurrentDialogueChoices is { Count: > 0 }) ");
            if (ChoicesSelection.style.display == DisplayStyle.Flex)
            {
                DoConfirm();
                HideChoicesSelection();
            }
            else
            {
                ShowChoicesSelection();
            }

        }
        else
        {
            Debug.Log("Branch: else ");

            if(HideAfterLastLine == true)
                ClosePopup();
        }
    }

    protected void ShowChoicesSelection()
    {
        ChoicesSelection.style.display = DisplayStyle.Flex;
        ChoicesSelection.focusable = true;
        // ChoicesSelection.Focus();
        ResetFocus();
    }

    protected void HideChoicesSelection()
    {
        // ChoicesSelection.itemsSource = null;
        // ChoicesSelection.Rebuild();
        ChoicesSelection.style.display = DisplayStyle.None;
        ChoicesSelection.focusable = false;
    }

    protected void ResetFocus()
    {
        ChoicesSelection.Focus();
        if (ChoicesSelection.itemsSource.Count > 0)
        {
            ChoicesSelection.ClearSelection();
            ChoicesSelection.Blur();
            ChoicesSelection.Focus();
            ChoicesSelection.SetSelection(0);
        }
    }

    protected void SetInventoryList<T>(List<T> itemsToDisplay) where T : IDisplayableInformation
    {
        var listviewController = new ListviewController<T>();
        
        if (ChoicesEntryTemplate is null)
            ChoicesEntryTemplate = Resources.Load<VisualTreeAsset>(PATH_TO_CHOICES_ENTRY_TEMPLATE_VISUALTREEASSET);
        
        ChoicesSelection = listviewController.InitializeItemList(Root, ChoicesEntryTemplate, itemsToDisplay, OnItemSelected, "ChoicesSelection");
    }

    public void OnItemSelected(IEnumerable<object> selectedItems)
    {

    }

    protected void DoConfirm()
    {
        // Remove Focus from choices if the player selects an item in the list
        if (ChoicesSelection.focusController.focusedElement == ChoicesSelection)
        {
            SendBackSelectedChoice(ChoicesSelection.selectedItem as StoryChoice);
            ChoicesSelection.focusable = false;
        }
    }

    protected void ClosePopup()
    {
        Hide();
        NotifyNav(UIController.EInterface.ExplorationUIContext);
    }

}