using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using Animancer;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;


namespace BattleV3
{
    public class BattlePlayerController : MonoBehaviour, IBattlerMovement
    {
        private static readonly int DirX = Animator.StringToHash("DirX");
        private static readonly int DirY = Animator.StringToHash("DirY");
        private static readonly int Moving = Animator.StringToHash("IsMoving");
        [field: SerializeField] public float MoveSpeed { get; set; }
        public Vector2 CurrentPosition => gameObject.transform.position;
        public Movement CurrentMovement { get; set; }
        public BattleSpawnPoint SpawnPoint { get; set; }

        public IBattlerMovement Movement => this;


        public bool IsMoving  { 
            get
            {
                if (CurrentMovement is null)
                    return false;

                if (CurrentMovement.ReachedDestination(transform.position))
                    return false;

                return true;
            }
        }

        public void MoveTo(Movement movement)
        {
            CurrentMovement = movement;
            transform.DOMove(movement.LocalPositionDestination, 0.5f);
        }

        public void SetPosition(Vector2 position)
        {
            transform.position = position;
        }

    }
}