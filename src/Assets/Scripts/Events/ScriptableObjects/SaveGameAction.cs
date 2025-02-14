using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using WorldPersistence;

namespace GameActions
{
    [CreateAssetMenu(menuName = "StoryChoiceActions/SaveGameAction")]

    public class SaveGameAction : GameAction
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public override void DoAction()
        {
            var DataMngr = FindObjectOfType<DataManager>();
            var WorldData = FindObjectOfType<WorldDataController>();
            DataMngr.SaveGame(DataMngr.ActiveSaveSlot, DataMngr.Player);
            WorldData.SaveScenePersistentData();
            WorldData.CommitWorldData();
        }
    }
}