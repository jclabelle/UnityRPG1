using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class ChoicesEntryController
{
    Label m_NameLabel { set; get; }

    public void SetVisualElement(VisualElement visualElement)
    {
        m_NameLabel = visualElement.Q<Label>("EntryName");
    }

    public void SetItemData(Dialogues.StoryChoice itemData)
    {
        m_NameLabel.text = itemData.GetDisplayableDescription();
    }
}