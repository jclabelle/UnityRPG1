using System.Collections.Generic;
using System.Linq;
using Quests;

public class RewardsManagement : IQuestInterface
{
    DataManager dataMngr;
    public DataManager DataMngr { get => dataMngr; set => dataMngr = value; }

    public void SetDataManager(DataManager d)
    {
        DataMngr = d;
    }

    //todo: Code EventQuest
    public bool EventQuest(QuestManagement qc, Quest q = null, Objective o = null)
    {
        return false;   
    }

    public void Add(IReward.Reward reward)
    {
        DataMngr.Player.Gold += reward.Gold;
        DataMngr.Player.XP += reward.XP;

        if (reward.Loot is List<Item> items)
        {
            List<IInventoryItem> i = new List<IInventoryItem>();
            i.AddRange(items);
            dataMngr.PlayerInventory.AddToInventory(i);
        }

        if (reward is IReward.SpecialReward special)
        {
            if (special.QuestObjectives is List<Objective> objectives)
            {
                foreach (Objective objective in objectives)
                {
                    if (objective.quest.IsActive(DataMngr.PlayerQuests) == true)
                    {
                        EventQuest(DataMngr.PlayerQuests, objective.quest, objective);
                    }
                }
            }

            if (special.Abilities is List<AbilityV2> abilities)
                DataMngr.Player.Abilities.AddRange(abilities.Except(DataMngr.Player.Abilities));

            if (special.Reactions is List<ReactionV2> reactions)
                DataMngr.Player.Reactions.AddRange(reactions.Except(DataMngr.Player.Reactions));
        }
        
    }

}