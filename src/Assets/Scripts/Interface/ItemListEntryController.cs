using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class ItemListEntryController
{
    Label m_NameLabel { set; get; }

    public void SetVisualElement(VisualElement visualElement)
    {
        m_NameLabel = visualElement.Q<Label>("ItemName");
    }

    public void SetItemData(Item itemData)
    {
        m_NameLabel.text = itemData.GetDisplayableName();
    }

}