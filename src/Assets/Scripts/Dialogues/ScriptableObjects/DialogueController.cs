using System;
using System.Collections;
using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine.UIElements;
#endif

namespace Dialogues
{
    [CreateAssetMenu(menuName = "Dialogue")]

    public class DialogueController : ScriptableObject
    {
        [field: SerializeField] public Dialogue DialogueInstance { get; set; }
        public bool IsDone => DialogueInstance.LinesIndex >= DialogueInstance.Lines.Count;
        public void ResetProgress() => DialogueInstance.ResetIndex();
        public Line GetNextLine() => DialogueInstance.GetNextLine();
        public static DialogueController InstantiateAnonOneLiner(string line)
        {
            var dialogue = CreateInstance<DialogueController>();
            var oneliner = Dialogue.CreateEmpty();
            oneliner.AddLine(Line.Create(Line.EType.Anonymous, line, String.Empty));
            dialogue.DialogueInstance = oneliner;
            return dialogue;
        }

#if UNITY_EDITOR
        [SerializeField] [TextArea(5, 5)] string warnings;

        private void OnValidate()
        {

            foreach (var l in DialogueInstance.Lines)
            {
                if (l.Type == Line.EType.Anonymous && l.Speaker != Line.AnonymousLine)
                {
                    l.speaker = Line.AnonymousLine;
                    Debug.Log($"{DateTime.Now} Dialogue {this.name}: line is set to Anonymous, deleting name");
                    warnings = $"{DateTime.Now}\nDialogue {this.name}: line is set to Anonymous, deleting name";
                }
            }
        }

        public Dialogue GetGameDialogue()
        {
            return DialogueInstance;
        }
#endif
    }
}