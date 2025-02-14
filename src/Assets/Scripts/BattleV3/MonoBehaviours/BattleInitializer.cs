using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BattleV3;
using UnityEngine;

namespace BattleV3
{
    public class BattleInitializer : MonoBehaviour, IInitializeBattle
    {
        [field: SerializeField] public BattlerData PlayerBattleData { get; set; }
        public BattleEncounter Encounter { get; set; }

        private void Start()
        {
            Debug.Log("Initializer called");
            Battle.BattleLock.ForceGrabLock();

            // todo: Check if this is needed. Was used to debug issues with potential multiple initializers
            var count = FindObjectsOfType<BattleInitializer>();
            var number = count.Length;

            var playerData = FindObjectOfType<DataManager>().Player;
            var battleEncounter = FindObjectOfType<WorldDataController>().NextBattleEncounter;
            Encounter = battleEncounter;
            SetPlayerBattler(playerData);
            MakeBattlers(battleEncounter);
            
            Battle.BattleLock.ReleaseLock();
        }

        public void SetPlayerBattler(PlayerData playerData)
        {
            PlayerBattleData.SetValues(playerData.CurrentStats, playerData.Abilities, playerData.Reactions);
            var player = GameObject.FindWithTag("Player");
            var playerBattler = player.AddComponent<Battler>();
            playerBattler.SetValues(PlayerBattleData);
            playerBattler.BattlerMaterial.SetMaterial(BattleFX.BattlerDefaultMaterial);

            var playerController = player.GetComponent<BattlePlayerController>();
            var playerAnimations = player.GetComponent<Animations.BattlerAnimationsController>();
            GameObject.FindWithTag("PlayerSpawnPoint").GetComponent<BattleSpawnPoint>().SetPositionAndFacing(
                (Animations.IBattlerAnimations)playerAnimations, (IBattlerMovement)playerController);
        }

        public List<Battler> MakeBattlers(BattleEncounter encounter)
        {
            var battlers = new List<Battler>(encounter.Enemies.Count);
            foreach (var enemyData in encounter.Enemies)
            {
                GameObject battlerObject = new GameObject(IDisplayableInformation.GetScriptableObjectDisplayableName(enemyData), typeof(Battler));
                var battler = battlerObject.GetComponent<Battler>();
                battler.SetValues(enemyData);
                var spriteRenderer = battler.GetComponent<SpriteRenderer>();
                spriteRenderer.sprite = enemyData.Sprite;
                spriteRenderer.sortingLayerName = BattleFX.PlayerSortingLayerName;
                spriteRenderer.sortingOrder = BattleFX.PlayerSortingOrder;
                spriteRenderer.material = BattleFX.BattlerDefaultMaterial;
                battlers.Add(battler);
            }

            var allSpawnPoints = FindObjectsOfType<BattleSpawnPoint>().ToList();
            var npcSpawnPoints = allSpawnPoints.Where(spawnPoint => !spawnPoint.CompareTag("PlayerSpawnPoint")).ToList();
            var playerSpawnPoint = allSpawnPoints.Find(spawnPoint => spawnPoint.CompareTag("PlayerSpawnPoint"));
            
            if (allSpawnPoints.Count < battlers.Count)
                throw new Exception("Not enough Spawn Points found to spawn all Battlers");
            
            foreach (var battler in battlers)
            {
                var spawnPoint = npcSpawnPoints[UnityEngine.Random.Range(0, npcSpawnPoints.Count)];
                
                battler.transform.position = spawnPoint.transform.position;
                if(battler.transform.position.x < playerSpawnPoint.transform.position.x &&
                   battler.BattlerGraphicsData.SpriteAppearsToLookTowards is BattlerData.ESpriteFacing.Left)
                    battler.BattlerGraphicsData.FlipSpriteX();
                
                npcSpawnPoints.Remove(spawnPoint);
            }

            return battlers;
        }
    }

    public interface IInitializeBattle
    {
        public void SetPlayerBattler(PlayerData playerData);
        public List<Battler> MakeBattlers(BattleEncounter encounter);
        public BattleEncounter Encounter { get; set; }
    }
}