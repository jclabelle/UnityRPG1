using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Dialogues
{
    public class DialoguePlayer : MonoBehaviour, IDialogue
    {
        IPopupDialogue popupDialogue;

        [field: SerializeField] private DialogueTree Tree { get; set; }
        public StoryChoice CurrentChoice { get; set; }

        protected void Start()
        {
            popupDialogue = FindObjectOfType<PopupDialogue>().GetComponent<PopupDialogue>();
        }

    

        public void PlayEvent(DialogueTree tree)
        {
            Tree = tree;
            StartCoroutine(PlayDialogueTree());
        }

        public void SetCurrentChoice(StoryChoice choice)
        {
            CurrentChoice = choice ?? throw new SystemException(
                $"Scene {SceneManager.GetActiveScene()}, Branchingdialogues, SetCurrentChoice: Choice is Null");

            if (CurrentChoice.GameAction != null)
            {
                CurrentChoice.GameAction.DoAction();
            }
        }

        public IEnumerator PlayDialogueTree()
        {
            var trunk = new StoryChoice();
            foreach (var sc in Tree.AllChoices)
            {
                if (sc.Name == "Trunk")
                {
                    trunk = sc;
                }
            }

            DialogueTreeBranch currentBranch = Tree.GetBranch(trunk);
            CurrentChoice = trunk;
            popupDialogue.StartDialogue(currentBranch, SetCurrentChoice);

            while (currentBranch.HasSplits is true)
            {
                if (Tree.ChoiceIsBranchTrigger((CurrentChoice)))
                {
                    if (currentBranch.Equals(Tree.GetBranch(CurrentChoice)) is false)
                    {
                        currentBranch = Tree.GetBranch(CurrentChoice);
                        popupDialogue.StartDialogue(currentBranch, SetCurrentChoice);
                    }
                }
                else
                {
                    popupDialogue.EndDialogue();
                    break;
                }

                yield return null;
            }

            Debug.Log("Finished Dialogue While Loop");

        }

        public void PlayDialogueV2(DialogueController dialogue)
        {
            popupDialogue.StartDialogue(dialogue);
        }



        public void PlayDialogue(string text)
        {
            var dialogue = ScriptableObject.CreateInstance<DialogueController>();
            dialogue.DialogueInstance.AddLine(new Line(Line.EType.Anonymous, text, ""));
            PlayDialogueV2(dialogue);
        }

        public void PlayDialogue(string text, string speakerName)
        {
            var dialogue = ScriptableObject.CreateInstance<DialogueController>();
            dialogue.DialogueInstance.AddLine(new Line(Line.EType.Conversation, text, speakerName));
            PlayDialogueV2(dialogue);
        }

        public void PlayDialogue(DialogueTree tree)
        {
            PlayEvent(tree);
        }

        public void PlayDialogue(DialogueController dialogue)
        {
            throw new NotImplementedException();
        }
    }

    public interface IDialogue
    {
        public void PlayDialogue(string text);
        public void PlayDialogue(DialogueTree tree);
        public void PlayDialogue(DialogueController dialogue);
        public void PlayDialogue(string text, string speakerName);


    }
}