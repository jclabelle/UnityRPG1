using System.Collections;
using System.Collections.Generic;
using Dialogues;
using UnityEngine;

namespace Quests
{
    public class QuestCheckpoint : MonoBehaviour, IQuestInterface, IEventProbeActive
    {
        public Quest quest;
        public Objective objective;

        [SerializeField] private DialogueQuest lines;


        public DialogueQuest Lines
        {
            get => lines;
            set => lines = value;
        }

        public IDialogue DialogueBox { get; set; }




        public bool EventQuest(QuestManagement qc, Quest q = null, Objective o = null)
        {

            Debug.Log("Trying: " + this.GetType() + " with Quest: " + quest.name + " for Objective: " + objective.name);

            // If this is an active quest
            if (quest.IsActive(qc) == true)
            {
                // If the objective is not yet complete, add it to the Active quest's objectives completed list

                if (quest.IsObjectiveCompleted(qc, objective) == false)
                {
                    qc.AddCompletedObjective(quest, objective);
                    Debug.Log("Success: " + this.GetType() + " with Quest: " + quest.name + " for Objective: " +
                              objective.name);
                    return true;
                }
            }

            return false;

        }

        public void EventProbeLaunched(GameObject player)
        {
            var dataMngr = FindObjectOfType<DataManager>();

            if (EventQuest(dataMngr.PlayerQuests) == true && Lines.Complete is string objectiveDone)
            {
                DialogueBox.PlayDialogue(objectiveDone);
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
            if (quest is Quest q && objective is Objective o)
            {
                if (o.quest is Quest oq)
                {
                    if (q.IsSame(oq) == false)
                        Debug.Log(
                            $" WARNING: {this.name} in {gameObject.name}: objective.quest({objective.quest.name}) and quest({quest.name}) do not match");
                }
                else
                {
                    Debug.Log($" WARNING: {o.name} in {this.name} has no Quest assigned");
                }

            }

            if (quest == null)
                Debug.Log($" WARNING: {this.name} in {gameObject.name} has no Quest assigned");

            if (objective == null)
                Debug.Log($" WARNING: {this.name} in {gameObject.name} has no Objective assigned");

            if (DialogueBox is null)
                DialogueBox = gameObject.GetComponent<DialoguePlayer>();
        }
    }
}


