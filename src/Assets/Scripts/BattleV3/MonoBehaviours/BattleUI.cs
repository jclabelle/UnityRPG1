using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace BattleV3
{
    public class BattleUI : MonoBehaviour, IBattleUI
    {
        private UIController uiController;
        [field: SerializeField] private VisualTreeAsset Popup { get; set; }
        [field: SerializeField] private PanelSettings PanelSettings { get; set; }
        private (List<Battler>, AbilityV2) PlayerChoice { get; set; }
        private ReactionV2 PlayerReaction { get; set; }
        private GameObject activePopup;
        private List<GameObject> activeTextPopups;

        // Start is called before the first frame update
        void Start()
        {
            uiController = FindObjectOfType<UIController>();
            
            StartCoroutine(ChangeUIContextAtEndOfFrame(UIController.EInterface.BattleSelectReaction));
        }
        private IEnumerator ChangeUIContextAtEndOfFrame(UIController.EInterface eInterface)
        {
            yield return new WaitForEndOfFrame();
            uiController.ChangeContext(eInterface);
        }


        void IBattleUI.ShowBattleSelectActionMenu()
        {
            if (uiController.PlayerIsChoosingBattleAction is false)
            {
                uiController.ChangeContext(UIController.EInterface.BattleSelectMenu);
            }
        }

        public void HideBattleSelectActionMenu()
        {
            if (uiController.PlayerIsChoosingBattleAction is true)
            {
                uiController.ChangeContext(UIController.EInterface.BattleSelectReaction);
            }

        }

        public ReactionV2 TryGetPlayerReaction()
        {
            if (PlayerReaction is null) 
                return null;
            
            var reaction = PlayerReaction;
            PlayerReaction = null;
            return reaction;
        }

        public void SetPlayerAction((List<Battler>, AbilityV2) choice)
        {
            PlayerChoice = choice;
        }

        public void SetPlayerReaction(ReactionV2 reaction)
        {
            PlayerReaction = reaction;
        }

        (List<Battler>, AbilityV2) IBattleUI.TryGetPlayerAction()
        {
            if (PlayerChoice is (null, null))
                return (null, null);

            var item2 = PlayerChoice.Item2;
            if (item2 is null)
                return (null, null);

            var choice = PlayerChoice;
            PlayerChoice = (null, null);
            return choice;
        }

        public void ShowActionNamePopup(Vector3 position, string text)
        {
            GameObject battlePopup = new GameObject("BattlePopup", typeof(UIDocument));
            battlePopup.transform.position = new Vector3(position.x, position.y, -9);
            activePopup = battlePopup;
            var uiDoc = battlePopup.GetComponent<UIDocument>();
            uiDoc.visualTreeAsset = Popup;
            uiDoc.panelSettings = PanelSettings;
            uiDoc.rootVisualElement.Q<Label>("DialogueText").text = text;
        }
        
        public void ShowTextPopup(Vector3 position, string text, float duration)
        {
            GameObject battlePopup = new GameObject("BattlePopup", typeof(UIDocument));
            battlePopup.transform.position = new Vector3(position.x, position.y, -9);
            var uiDoc = battlePopup.GetComponent<UIDocument>();
            uiDoc.visualTreeAsset = Popup;
            uiDoc.panelSettings = PanelSettings;
            uiDoc.rootVisualElement.Q<Label>("DialogueText").text = text;
            activeTextPopups.Add(battlePopup);
        }

        public void HideActionNamePopup()
        {
            Destroy(activePopup);
            activePopup = null;
        }
    }

    public interface IBattleUI
    {
        public void ShowBattleSelectActionMenu();
        public void HideBattleSelectActionMenu();
        public (List<Battler>, AbilityV2) TryGetPlayerAction();
        public ReactionV2 TryGetPlayerReaction();
        public void SetPlayerAction((List<Battler>, AbilityV2) choice);
        public void SetPlayerReaction(ReactionV2 reaction);

        void ShowActionNamePopup(Vector3 location,string text);
        void HideActionNamePopup();
    }
}