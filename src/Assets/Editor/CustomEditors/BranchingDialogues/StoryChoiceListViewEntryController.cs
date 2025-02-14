using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using Dialogues;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class StoryChoiceListViewEntryController<T> where T: IDisplayableInformation
{
    private Label NameLabel { set; get; } 
    private TextField Prompt { get; set; }
    private Label Traits { get; set; }
    private Button AddTrait { get; set; }
    private Button RemoveTrait { get; set; }
    private DropdownField TraitsEnum { get; set; }   
    
    private StoryChoice Choice { get; set; }

    public void SetVisualElement(VisualElement visualElement)
    {
        NameLabel = visualElement.Q<Label>("EntryName");
        Prompt = visualElement.Q<TextField>("ChoicePrompt");
        Traits = visualElement.Q<Label>("ChoiceTraits");
        AddTrait = visualElement.Q<Button>("AddTrait");
        RemoveTrait = visualElement.Q<Button>("RemoveTrait");
        TraitsEnum = visualElement.Q<DropdownField>("TraitsEnum");
    }

    public void SetItemData(T itemData)
    {
        if (itemData is StoryChoice choice)
        {
            NameLabel.text = choice.Name;
            Prompt.value = choice.Prompt;
            foreach (var trait in choice.Traits)
            {
                Traits.text += trait.ToString() + "  ";
            }
        }
    }

}