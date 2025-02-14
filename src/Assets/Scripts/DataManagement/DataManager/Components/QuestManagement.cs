using System.Collections.Generic;
using UnityEngine;

namespace Quests
{
    public class QuestManagement
    {

        DataManager dataMngr;

        public DataManager DataMngr
        {
            get => dataMngr;
            set => dataMngr = value;
        }


        public void SetDataManager(DataManager d)
        {
            DataMngr = d;
        }

        public void AddActive(Quest q)
        {
            DataMngr.Player.ActiveQuests.Add(q, new List<Objective>());
        }

        public Dictionary<Quest, List<Objective>> GetActives()
        {
            return DataMngr.Player.ActiveQuests;
        }

        // Adds an objective to the list of a CompletedQuests objectives for a quest.
        public void AddCompletedObjective(Quest quest, Objective objective)
        {
            List<Objective> objectives;
            if (DataMngr.Player.ActiveQuests.TryGetValue(quest, out objectives))
            {
                objectives.Add(objective);
            }
            else
            {
                Debug.Log($"{this} failed to add {objective} for key {quest} to List {DataMngr.Player.ActiveQuests} ");
            }

        }

        public List<Quest> GetCompleted()
        {
            return DataMngr.Player.CompletedQuests;
        }

        public void MoveActiveToCompleted(Quest q)
        {
            DataMngr.Player.ActiveQuests.Remove(q);
            DataMngr.Player.CompletedQuests.Add(q);
        }

    }
}