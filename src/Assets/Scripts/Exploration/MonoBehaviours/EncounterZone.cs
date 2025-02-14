using System.Collections;
using System.Linq;
using BattleV3;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using Exploration;


//Overrides default battle data from GameSceneData in the GameSceneController on collision.
namespace Exploration
{
    [RequireComponent(typeof(BoxCollider2D), typeof(Rigidbody2D))]

    public class EncounterZone : MonoBehaviour
    {
        [SerializeField] Sprite battleBackground;
        [SerializeField] int encounterChancePerStep;
        [SerializeField] private EncounterTable randomEncounters;

        public Sprite BattleBackground
        {
            get => battleBackground;
            set => battleBackground = value;
        }

        public EncounterTable RandomEncounters
        {
            get => randomEncounters;
            set => randomEncounters = value;
        }

        public int EncounterChancePerStep
        {
            get => encounterChancePerStep;
            set => encounterChancePerStep = value;
        }

        private void Awake()
        {
            Assert.IsTrue(randomEncounters.WeightsLessOrEqualOne() is true,
                $"Weights for Encounters in {gameObject.name} in {SceneManager.GetActiveScene().name} are greater than 1");
        }


        // Start is called before the first frame update
        void Start()
        {
            Assert.IsTrue(FindObjectOfType<ExplorationSceneController>() is ExplorationSceneController);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public BattleEncounter GetEncounter()
        {
            return RandomEncounters.Roll().First();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.name == "Player")
            {
                FindObjectOfType<ExplorationSceneController>().SetActiveEncounterZone(this);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.name == "Player")
            {
                FindObjectOfType<ExplorationSceneController>().RemoveActiveEncounterZone(this);
            }
        }
    }
}