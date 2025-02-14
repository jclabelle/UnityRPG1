using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Quests
{
    [CreateAssetMenu, System.Serializable]

    public class Quest : ScriptableObject, ISavePlayerData
    {

        /// <summary>
        /// Quest specific
        /// </summary>
        public Objective[] objectives;

        public Quest[] prerequisites;
        public string description;
        public IReward.Reward Prize { set; get; }


        public bool IsSame(Quest q)
        {
            return this.name.Trim() == q.name.Trim();

        }

        // Has the player completed this quest?
        public bool IsReadyToComplete(QuestManagement qc)
        {
            List<Objective> completedObjectives; //= new List<Objective>();

            if (qc.GetActives().TryGetValue(this, out completedObjectives))
            {
                foreach (Objective o in objectives)
                {
                    if (completedObjectives.Contains(o) == false)
                    {
                        return false;
                    }
                }
            }

            return true;

        }

        // Can the player start this quest?
        public bool CanStart(QuestManagement qc)
        {
            // If this is not completed or active, and has requirements.
            if (IsCompleted(qc) == false && IsActive(qc) == false)
            {
                if (prerequisites.Length > 0)
                {
                    foreach (Quest q in prerequisites)
                    {
                        if (qc.GetCompleted().Contains(q) == false)
                        {
                            return false; // Return false if we find a missing requirement
                        }
                    }

                    return true; // Return true if we loop through Completed and find all prerequisites
                }
                else
                {
                    return true; // return true if the quest has no prerequisites
                }

            }

            return false; // return false if the quest is either completed or active
        }

        // Is this quest in the player's completed list?
        public bool IsCompleted(QuestManagement qc)
        {
            return qc.GetCompleted().Contains(this);
        }

        // Is this quest in the player's active list?
        public bool IsActive(QuestManagement qc)
        {
            return qc.GetActives().ContainsKey(this);
        }

        public bool IsObjectiveCompleted(QuestManagement qc, Objective objective)
        {
            List<Objective> completedObjectives;

            if (qc.GetActives().TryGetValue(this, out completedObjectives))
            {
                if (completedObjectives.Contains(objective))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Rewards
        /// </summary>

        [SerializeField] private int gold;

        [SerializeField] private int xp;
        [SerializeField] private List<IInventoryItem> items;
        [SerializeField] private List<Objective> questObjectives;
        [SerializeField] private List<AbilityV2> abilities;
        [SerializeField] private List<ReactionV2> reactions;

        public List<IInventoryItem> Loot
        {
            get => items;
            set => items = value;
        }

        public int Gold
        {
            get => gold;
            set => gold = value;
        }

        public int XP
        {
            get => xp;
            set => xp = value;
        }

        public List<Objective> QuestObjectives
        {
            get => questObjectives;
            set => questObjectives = value;
        }

        public List<AbilityV2> Abilities
        {
            get => abilities;
            set => abilities = value;
        }

        public List<ReactionV2> Reactions
        {
            get => reactions;
            set => reactions = value;
        }

        public IReward.Reward GetReward()
        {
            return new IReward.Reward();
        }

        public void ResetReward()
        {
            Loot = new List<IInventoryItem>();
            Gold = 0;
            XP = 0;
            QuestObjectives = new List<Objective>();
            Abilities = new List<AbilityV2>();
            Reactions = new List<ReactionV2>();
        }


        /// <summary>
        /// Save Game
        /// </summary>
        private string saveGameName;

        private string saveGameType;

        public string SaveGameName
        {
            get => saveGameName;
            set => saveGameName = value;
        }

        public string SaveGameType
        {
            get => saveGameType;
            set => saveGameType = value;
        }

        public void SetSaveGameData()
        {
            SaveGameName = $"{this}".Trim();
            SaveGameType = $"{this.GetType()}".Trim();
        }

        protected void OnEnable()
        {
            SetSaveGameData();
        }

        public virtual JObject GetSaveGameJson()
        {
            JObject JO = new JObject();
            JO.Add($"{nameof(SaveGameName)}", SaveGameName);
            JO.Add($"{nameof(SaveGameType)}", SaveGameType);

            return JO;
        }

    }

    public interface IQuestInterface
    {
        public bool EventQuest(QuestManagement qc, Quest q = null, Objective o = null);

    }
}

