using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.Assertions;
using UnityEngine.UIElements;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Dialogues
{
    [System.Serializable]
        [CreateAssetMenu(menuName = "Dialogue Tree")]

    public class DialogueTree : ScriptableObject
    {
        private Dictionary<StoryChoice, DialogueTreeBranch> tree;

        public Dictionary<StoryChoice, DialogueTreeBranch> Tree
        {
            get => tree;
            set => tree = value;
        }

        public StoryChoice Trunk { get; set; }

        public List<StoryChoice> AllChoices
        {
            get => allChoices;
            set => allChoices = value;
        }

        public List<DialogueTreeBranch> AllBranches
        {
            get => allBranches;
            set => allBranches = value;
        }

        [SerializeField] private List<StoryChoice> allChoices;
        [SerializeField] private List<DialogueTreeBranch> allBranches;

        public DialogueTreeBranch GetBranch(StoryChoice choice)
        {
            if (Tree is null)
                Stitch();
            if (Tree.ContainsKey(choice) is false)
                throw new ArgumentException(
                    $"Scene {SceneManager.GetActiveScene()}, DialogueTree {this.name}, prompt {choice.Prompt}:  Key not found.");
            return Tree[choice];
        }
        
        

        public bool ChoiceIsBranchTrigger(StoryChoice choice)
        {
            return Tree.ContainsKey(choice);
        }

        public Dictionary<StoryChoice, DialogueTreeBranch> EditorAccessTree()
        {
            return Tree;
        }

        private void Stitch()
        {
            Tree = new Dictionary<StoryChoice, DialogueTreeBranch>();

            for (int i = 0; i < AllChoices.Count; i++)
            {
                foreach (var branch in AllBranches)
                {
                    if (branch.IndexOfTriggeringChoiceInAllChoices == i)
                    {
                        Tree.Add(AllChoices[i], branch);
                    }
                }
            }

            //todo: Initialize branch Choices.
            foreach (var branch in AllBranches)
            {
                if (branch.HasSplits is true)
                {
                    branch.Choices = new List<StoryChoice>();
                    foreach (var index in branch.GetChoicesIndexes())
                    {
                        // if (branch.Choices is null)
                        // {
                        //     branch.Choices = new List<StoryChoice>();
                        // }
                        if (index < AllChoices.Count)
                        {
                            branch.Choices.Add(AllChoices[index]);
                        }
                        else
                        {
                            throw new UnityException("DialogueTree, Stitch: Index of Choice out of bounds");
                        }
                    }
                }
            }
        }

        private void Reset()
        {
            var trunkSC = new StoryChoice() { Prompt = "TRUNK", Name = "Trunk" };
            Trunk = trunkSC;
            AllChoices = new List<StoryChoice>();
            AllChoices.Add(Trunk);

            var trunkDTB = new DialogueTreeBranch();
            trunkDTB.DialogueComponent.DialogueInstance = Dialogue.CreateWithEmptyLines(1);
            AllBranches.Add((trunkDTB));    
        }

        public void Init()
        {
            Reset();
        }
    }
}

