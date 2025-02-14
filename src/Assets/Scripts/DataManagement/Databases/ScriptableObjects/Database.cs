using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Data.Gameplay;
using UnityEditor;

namespace Data.Gameplay
{
    [CreateAssetMenu(menuName = "Databases/Database")]
    public class Database : ScriptableObject
    {
        [SerializeField] private BattleEncountersDB battleEncountersDB;
        [SerializeField] private ItemsDB itemsDB;
        [SerializeField] private AbilitiesDB abilitiesDB;
        [SerializeField] private ReactionsDB reactionsDB;
        [SerializeField] private QuestsDB questsDB;
        [SerializeField] private ObjectivesDB objectivesDB;

        public BattleEncountersDB BattleEncountersDB { get => battleEncountersDB; set => battleEncountersDB = value; }
        public ItemsDB ItemsDB { get => itemsDB; set => itemsDB = value; }
        public AbilitiesDB AbilitiesDB { get => abilitiesDB; set => abilitiesDB = value; }
        public ReactionsDB ReactionsDB { get => reactionsDB; set => reactionsDB = value; }
        public QuestsDB QuestsDB { get => questsDB; set => questsDB = value; }
        public ObjectivesDB ObjectivesDB { get => objectivesDB; set => objectivesDB = value; }

    }
}