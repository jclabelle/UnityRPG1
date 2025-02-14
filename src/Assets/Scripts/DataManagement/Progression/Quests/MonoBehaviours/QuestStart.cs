using System.Collections;
using System.Collections.Generic;
using Dialogues;
using UnityEngine;

namespace Quests
{
    public class QuestStart : MonoBehaviour, IQuestInterface, IEventProbeActive
    {
        public Quest quest;
        [SerializeField] private DialogueQuest lines;


        public DialogueQuest Lines
        {
            get => lines;
            set => lines = value;
        }

        public IDialogue DialogueBox { get; set; }

  

        public bool EventQuest(QuestManagement qc, Quest q = null, Objective o = null)
        {
            Debug.Log("Trying: " + this.GetType() + " with Quest: " + quest.name);

            // If the quest can start, add it to the active quest list
            if (quest.CanStart(qc) == true)
            {
                qc.AddActive(quest);
                Debug.Log("Success: " + this.GetType() + " with Quest: " + quest.name);
                return true;
            }

            return false;
        }

        public void EventProbeLaunched(GameObject player)
        {
            var dataMngr = FindObjectOfType<DataManager>();

            if (EventQuest(dataMngr.PlayerQuests) == true && Lines.Complete is string start)
            {
                DialogueBox.PlayDialogue(start);
            }
            else if (quest.IsActive(dataMngr.PlayerQuests) == true && Lines.During is string keepGoing)
            {
                DialogueBox.PlayDialogue(keepGoing);
            }
            else if (quest.IsCompleted(dataMngr.PlayerQuests) == true && Lines.After is string finished)
            {
                DialogueBox.PlayDialogue(finished);
            }
            else if (Lines.Before is string notYet)
            {
                DialogueBox.PlayDialogue(notYet);
            }
        }


        // Start is called before the first frame update
        void Start()
        {
            if (quest == null)
                Debug.Log($" WARNING: {this.name} in {gameObject.name} has no Quest assigned");
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
