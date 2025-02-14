using System.Collections;
using System.Collections.Generic;
using Dialogues;
using UnityEngine;

namespace Quests
{
    public class QuestEnd : MonoBehaviour, IQuestInterface, IEventProbeActive
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


            // If this is an active quest
            if (quest.IsActive(qc) == true)
            {
                // If the quest is ready to complete, move it from the active list to the completed list
                if (quest.IsReadyToComplete(qc) == true)
                {
                    qc.MoveActiveToCompleted(quest);
                    Debug.Log("Success: " + this.GetType() + " with Quest: " + quest.name);
                    return true;
                }
            }

            return false;
        }

        public void EventProbeLaunched(GameObject player)
        {
            var dataMngr = FindObjectOfType<DataManager>();
            if (EventQuest(dataMngr.PlayerQuests) == true && Lines.Complete is string congrats)
            {
                DialogueBox.PlayDialogue(congrats);
            }
            else if (quest.IsActive(dataMngr.PlayerQuests) == true && Lines.During is string keepGoing)
            {
                DialogueBox.PlayDialogue(keepGoing);
            }
            else if (quest.IsCompleted(dataMngr.PlayerQuests) == true && Lines.After is string completed)
            {
                DialogueBox.PlayDialogue(completed);
            }
            else if (Lines.Before is string notYet)
            {
                DialogueBox.PlayDialogue(notYet);
            }
        }

        private void Awake()
        {
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
