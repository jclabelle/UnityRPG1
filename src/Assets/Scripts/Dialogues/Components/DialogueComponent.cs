using System;
using UnityEngine;

namespace Dialogues
{
    [Serializable]
    public class DialogueComponent
    {
        [field: SerializeField] public Dialogue DialogueInstance { get; set; }
        public bool IsDone => DialogueInstance.LinesIndex >= DialogueInstance.Lines.Count;

        public void ResetProgress() => DialogueInstance.ResetIndex();
        public Line GetNextLine() => DialogueInstance.GetNextLine();
        public void SetDialogue(Dialogue dialogue) => this.DialogueInstance = dialogue;

        public DialogueController ToDialogueV2()
        {
            DialogueController dialogObject = ScriptableObject.CreateInstance<DialogueController>();
            dialogObject.DialogueInstance = DialogueInstance;
            return dialogObject;
        }
        public DialogueComponent() => DialogueInstance = new Dialogue();
        public DialogueComponent(Dialogue dialogue) =>  this.DialogueInstance = dialogue;
        public static DialogueComponent Create(Dialogue dialogue) => new DialogueComponent(dialogue);
        public static DialogueComponent CreateEmpty() => new DialogueComponent(Dialogue.CreateEmpty());
        public static DialogueComponent CreateWithEmptyLines(int countOfEmptyLines) => Create(Dialogue.CreateWithEmptyLines(countOfEmptyLines));
      
    }
}