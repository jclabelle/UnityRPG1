using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogues
{
    [Serializable]
    public class DialogueTreeBranch
    {
        [field: SerializeField] public DialogueComponent DialogueComponent { get; set; }
        [field: SerializeField] private List<int> IndexesOfChoicesInAllChoices { get;  set; }
        [field: SerializeField] public int IndexOfTriggeringChoiceInAllChoices { get; private set; }
        public List<StoryChoice> Choices { get; set; } = new List<StoryChoice>();
        public bool HasSplits=> IndexesOfChoicesInAllChoices.Count > 0;
        public List<int> GetChoicesIndexes() => IndexesOfChoicesInAllChoices;
        public void SetDialogue(DialogueComponent dialogueComponent) => DialogueComponent = dialogueComponent;
        public DialogueTreeBranch() => DialogueComponent = new DialogueComponent();
        public DialogueTreeBranch(DialogueComponent dialogueComponent, List<StoryChoice> choices) => DialogueComponent = dialogueComponent;
        public static DialogueTreeBranch Create() => new DialogueTreeBranch();
        public static DialogueTreeBranch Create(DialogueComponent dialogueComponent, List<StoryChoice> choices) => new DialogueTreeBranch(dialogueComponent, choices);
        public static DialogueTreeBranch CreateWithEmptyDialogueLines(int countOfEmptyLines, List<StoryChoice> choices) => Create(DialogueComponent.CreateWithEmptyLines(countOfEmptyLines), choices);
        public static DialogueTreeBranch CreateWithoutSplits(DialogueComponent dialogueComponent) => Create(dialogueComponent, null);

    }
}