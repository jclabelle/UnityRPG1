using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BattleV3
{
    public class BattleSpawnPoint : MonoBehaviour
    {
        [field: SerializeField] public float SpriteScaling { get; set; }
        //Direction of the battler's sprite so that it looks towards the player spawn point
        [field: SerializeField] public Vector2 LookDirection { get; set; }
        public Vector2 Position => transform.position;

        private void Awake()
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = null;
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetPositionAndFacing(Animations.IBattlerAnimations battlerAnimations, IBattlerMovement battlerMovement)
        {
            var lookDirection = new Vector2(
                Mathf.Clamp(LookDirection.x, -1.0f, 1f),
                Mathf.Clamp(LookDirection.y, -1, 1));

            battlerMovement.SpawnPoint = this;

            // Wait for EOF so the Animator is set in the Player Controller.
            StartCoroutine(SetPositionAtEof(battlerMovement));
            StartCoroutine(SetLookDirectionAtEof(battlerAnimations, lookDirection));
        }

        private IEnumerator SetPositionAtEof(IBattlerMovement battlerMovement)
        {
            yield return new WaitForEndOfFrame();
            battlerMovement.SetPosition(gameObject.transform.position);
        }
        
        private IEnumerator SetLookDirectionAtEof(Animations.IBattlerAnimations battlerAnimations, Vector2 lookDirection)
        {
            yield return new WaitForEndOfFrame();
            battlerAnimations.LookAt(lookDirection);
        }
    }
}