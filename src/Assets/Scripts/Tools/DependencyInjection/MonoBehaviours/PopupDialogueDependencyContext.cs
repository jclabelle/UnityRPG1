using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

public class PopupDialogueDependencyContext : MonoBehaviour
{
    [SerializeField]
    private PopupDialogue popupDialogue = default;

    private void Awake()
    {
        DependenciesContext.Dependencies.Add(new Dependency
        {
            Type = typeof(Dialogues.IPopupDialogue),
            Factory = () => Instantiate(popupDialogue).GetComponent<PopupDialogue>(),
            IsSingleton = true
        });
    }
}