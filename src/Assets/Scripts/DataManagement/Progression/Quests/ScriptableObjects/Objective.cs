using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Quests
{
    [CreateAssetMenu, System.Serializable]

    public class Objective : ScriptableObject
    {
        public string completedDialogue;
        public string description;
        public Quest quest;

        public bool IsSame(Objective o)
        {
            return this.Equals(o);
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
}
