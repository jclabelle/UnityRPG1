using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Dialogues;
using UnityEngine;
using UnityEngine.Assertions;


[CreateAssetMenu(menuName = "Dialogues/DialogueQuest")]
public class DialogueQuest : ScriptableObject
{
    [field: SerializeField] public string Before { get; set; } = "Text Missing"; // Before the quest is started
    [field: SerializeField] public string During { get; set; } = "Text Missing"; // While the quest is ongoing
    [field: SerializeField] public string Complete { get; set; } = "Text Missing"; // When the event completes - ie EventQuest returns True
    [field: SerializeField] public string After { get; set; } = "Text Missing"; // After the quest is completed.
}