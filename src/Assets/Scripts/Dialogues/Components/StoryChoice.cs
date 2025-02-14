using System;
using System.Collections.Generic;
using System.Linq;
using GameActions;
using UnityEngine;

namespace Dialogues
{
    [Serializable]
    public class StoryChoice : IDisplayableInformation
    {
        public enum StoryChoiceTrait
        {
            TempTrait,
            TempTrait2,
            TriggerVendor,
            EndDialogue,
        }
        

        [SerializeField] List<StoryChoiceTrait> traits;
        [SerializeField] public string prompt;
        [SerializeField] string name;

        private Func<object> Action { get; set; } = new Func<object>(() =>
        {
            Debug.Log($"Action not set in StoryChoice");
            return 0;
        });
       
        [field: SerializeField] private GameActions.GameAction StoryAction { get; set; }
        public IGameAction GameAction => StoryAction;

        public string Prompt
        {
            get => prompt;
            set => prompt = value;
        }

        public List<StoryChoiceTrait> Traits
        {
            get => traits;
            set => traits = value;
        }
        public string Name
        {
            get => name;
            set => name = value;
        }

        public void SetPrompt(string prompt) => Prompt = prompt;
        public void AddTrait(StoryChoiceTrait trait) => Traits.Add(trait);
        public void SetAction(Func<object> action) => Action = action;

        public string GetDisplayableName()
        {
            return Name;
        }

        public string GetDisplayableStats()
        {
            return string.Empty;
        }

        public string GetDisplayableDescription()
        {
            return Prompt;
        }

        public Texture2D GetDisplayableIcon()
        {
            return new Texture2D(25, 25);
        }

        public StoryChoice()
        {
        }

        public StoryChoice(string prompt, params StoryChoiceTrait[] traits)
        {
            this.Traits = traits.ToList();
            this.Prompt = prompt;
        }

        public StoryChoice(string prompt, Func<object> action, params StoryChoiceTrait[] traits)
        {
            this.Traits = traits.ToList();
            this.Prompt = prompt;
            this.Action = action;
        }

        public static StoryChoice Create()
        {
            return new StoryChoice();
        }

        public static StoryChoice Create(string prompt, params StoryChoiceTrait[] traits)
        {
            return new StoryChoice() { Prompt = prompt, Traits = traits.ToList() };
        }

        public static StoryChoice Create(string prompt, Func<object> action, params StoryChoiceTrait[] traits)
        {
            return new StoryChoice() { Prompt = prompt, Action = action, Traits = traits.ToList() };
        }
    }
}