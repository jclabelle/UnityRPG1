using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class ListviewEntryController<T> where T: IDisplayableInformation
{
    Label NameLabel { set; get; }

    public void SetVisualElement(VisualElement visualElement)
    {
        NameLabel = visualElement.Q<Label>("EntryName");
    }

    public void SetItemData(T itemData)
    {
        if(itemData is Dialogues.StoryChoice choice)
        {
            NameLabel.text = choice.GetDisplayableDescription();
        }
        else if (itemData is BattlePlayerMenuAction menuAction)
        {
            NameLabel.text = menuAction.GetDisplayableName();
        }
        else
        {
            NameLabel.text = itemData.GetDisplayableName();
        }

    }

}