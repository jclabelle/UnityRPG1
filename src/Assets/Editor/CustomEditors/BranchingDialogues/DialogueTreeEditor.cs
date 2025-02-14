using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System;

public class DialogueTreeEditor : EditorWindow
{
    public List<DialogueTree> allDialogueTrees;
    public Dictionary<string, DialogueTree> allDTDictionary = new Dictionary<string, DialogueTree>();
    public Dictionary<string, StoryChoice> allChoices = new Dictionary<string, StoryChoice>();
    public DialogueTree loadedTree;

    //static DialogueTreeEditor wnd;

    public VisualTreeAsset dialogueEntry;
    public VisualTreeAsset storychoiceEntry;
    public delegate void CallbackOnItemSelected(AdvancedDropdownItem item);

    public VisualElement menu;
    public VisualElement root;
    public DropdownField dropdownAllTrees;
    public Label loadedTreeName;
    public ScrollView scrollStoryChoices;
    public ScrollView scrollDialogues;
    public StoryChoiceListViewController<StoryChoice> listviewControllerStoryChoice;
    public StoryChoiceListViewEntryController<StoryChoice> listviewEntryControllerStoryChoice;

    public void SetUpUI()
    {
        var menuTemplate = EditorGUIUtility.Load("Assets/Editor Default Resources/CustomEditors/BranchingDialogues/DialogueTreeEditor.uxml") as VisualTreeAsset;
        var dialogueTemplate = EditorGUIUtility.Load("Assets/Editor Default Resources/CustomEditors/BranchingDialogues/ListViewEntryDialogue.uxml") as VisualTreeAsset;
        var storychoiceTemplate = EditorGUIUtility.Load("Assets/Editor Default Resources/CustomEditors/BranchingDialogues/ListViewEntryStoryChoice.uxml") as VisualTreeAsset;

        menu = menuTemplate.CloneTree();
        this.rootVisualElement.Add(menu);

        root = this.rootVisualElement;
       
        dropdownAllTrees = root.Q<DropdownField>("DialogueTreeSelector");
        dropdownAllTrees.choices = new List<string>(allDTDictionary.Keys);
        loadedTreeName = root.Q<Label>("LoadedTreeName");
        scrollStoryChoices = root.Q<ScrollView>("StoryChoices");
        scrollDialogues = root.Q<ScrollView>("Dialogues");

        //editChoicesListView = root.Q<ListView>("EditChoicesListView");

        //editChoicesListView = listviewControllerStoryChoice.InitializeItemList(root, storychoiceTemplate, )






        //dropdownAllTrees.RegisterCallback
        //Debug.Log(dropdownAllDialogueTrees.GetClasses());
        //Debug.Log(dropdownAllDialogueTrees.GetType());
        //Debug.Log(dropdownAllDialogueTrees.name);
    }

    protected void loadTree(DialogueTree dialogueTree)
    {
        loadedTree = dialogueTree;
        SetupListView();
        // editChoicesListView.itemsSource = dialogueTree.Tree.Keys.ToList<StoryChoice>();
        loadedTreeName.text = dialogueTree.name;
        
        Debug.Log("Loaded Tree:\n");
        Debug.Log(TreeDetails((loadedTree)));
    }

    // protected void SetupListView()
    // {
    //    var virtualTreeAsset = EditorGUIUtility.Load(
    //             "Assets/Editor Default Resources/CustomEditors/BranchingDialogues/ListViewEntryStoryChoice.uxml") as
    //         VisualTreeAsset;
    //    
    //     listviewControllerStoryChoice = new StoryChoiceListViewController<StoryChoice>();
    //     editChoicesListView = listviewControllerStoryChoice.InitializeItemList(
    //         root
    //         ,virtualTreeAsset
    //         , loadedTree.Tree.Keys.ToList<StoryChoice>()
    //         , OnItemSelectedChoices
    //         , "EditChoicesListView");
    // }

    public void OnItemSelectedChoices(IEnumerable<object> selectedItems)
    {
        // var selectedItem = editChoicesListView.selectedItem as IDisplayableInformation;

        if (selectedItem is null)
        {
            // Clear existing data
            //EntryStats.text = "";
            //EntryIcon.style.backgroundImage = null;
            return;
        }

        // Fill data
        //EntryStats.text = selectedItem.GetDisplayableName();
        //EntryIcon.style.backgroundImage = selectedItem.GetDisplayableIcon();
    }


    public void OnItemSelected(AdvancedDropdownItem item)
    {
        //Debug.Log(item.name.ToString().Count() - ("DialogueTree".Count() + 3));
        var itemNameTypeStripped
            = item.name.Remove(item.name.ToString().Count() - ("DialogueTree".Count() + 3));
        foreach (var tree in allDialogueTrees)
        {
            if (tree.name == itemNameTypeStripped)
            {
                loadedTree = tree;
                return;
            }
        }
        Debug.Log("Tree not found");

    }

    [MenuItem("Tools/Branching Dialogue Editor")]
    public static void ShowWindow()
    {
        var wnd = GetWindow<DialogueTreeEditor>("Branching Dialogue Editor");
        wnd.titleContent = new GUIContent("Dialogue Tree Edit");

    }

    public void OnEnable()
    {
        FindAllBranchingDialogues();
        SetUpUI();
    }

    private void OnGUI()
    {
        Debug.Log("Debug");
        //foreach (var k in allDTDictionary.Keys)
        //{
        //    Debug.Log(k);
        //}

        if(allDTDictionary[dropdownAllTrees.value] != loadedTree)
        {
            loadTree(allDTDictionary[dropdownAllTrees.value]);
        }
        Debug.Log(dropdownAllTrees.value);
        Debug.Log(allDTDictionary[dropdownAllTrees.value].name);

    }

    public void FindAllBranchingDialogues()
    {
        allDialogueTrees = AssetTools.GetAllAssetsOfType<DialogueTree>("DialogueTree");
        foreach(var dt in allDialogueTrees)
        {
            allDTDictionary.Add(dt.name, dt);
        }
    }

    public string RemoveTypeFromTreeName(DialogueTree tree)
    {
        return tree.name.Remove(tree.name.Count() - ("DialogueTree".Count() + 3));
    }

    protected string TreeDetails(DialogueTree tree)
    {
        if (tree is null)
            return new string("DialogueTree: Null");

        string details = String.Empty;
       details += tree.ToString();
        foreach(var branch in tree.EditorAccessTree())
        {
            if (branch.Value is null)
                details += "Dialogue Tree Branch: NULL";
        }

        return details;
    }

    protected string BranchDetails(DialogueTreeBranch branch)
    {
        if (branch is null)
            return new string("DialogueTreeBranch: Null");

        string details = String.Empty;
        string branchName = branch.ToString();
        string hasSplits = $"Splits: {branch.HasSplits}\n";
        return details;
    }

    protected string DialogueDetails(DialogueV2 dialogue)
    {
        if (dialogue is null)
            return new string("Dialogue: Null");

        string details = String.Empty;

        return details;
    }

    protected string ChoiceDetails(StoryChoice choice )
    {
        if (choice is null)
            return new string("StoryChoice: Null");

        string details = String.Empty;

        return details;
    }

    protected void AddBranch(DialogueTree tree, StoryChoice storyChoice)
    {
        DialogueTreeBranch newBranch = DialogueTreeBranch.CreateWithEmptyDialogueLines(1, null);
        tree.Tree.Add(storyChoice, newBranch);
    }

}

public class DialogueTreeDropdown : AdvancedDropdown
{

    public List<DialogueTree> allBranchingDialogues;
    private DialogueTreeEditor.CallbackOnItemSelected callbackOnItemSelected;

    public DialogueTreeEditor.CallbackOnItemSelected CallbackOnItemSelected { get => callbackOnItemSelected; set => callbackOnItemSelected = value; }

    public DialogueTreeDropdown(AdvancedDropdownState state) : base(state)
    {

    }

    protected override AdvancedDropdownItem BuildRoot()
    {
        var root = new AdvancedDropdownItem("Branching Dialogues");

        FindAllBranchingDialogues();

        foreach (var branchingDialogue in allBranchingDialogues)
        {
            root.AddChild(new AdvancedDropdownItem($"{branchingDialogue }"));
        }

        return root;
    }

    protected override void ItemSelected(AdvancedDropdownItem item)
    {
        CallbackOnItemSelected(item);
    }

    //public void SetCallbackOnItemSelected(Func<object> callback)
    //{
    //    callbackOnItemSelected += callback;
    //}

    public DialogueTreeEditor.CallbackOnItemSelected GetCallback()
    {
        return CallbackOnItemSelected;
    }

    public void FindAllBranchingDialogues()
    {
        allBranchingDialogues = AssetTools.GetAllAssetsOfType<DialogueTree>("DialogueTree");

    }
}