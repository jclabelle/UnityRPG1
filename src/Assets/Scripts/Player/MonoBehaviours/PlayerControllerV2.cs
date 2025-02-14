using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllerV2 : MonoBehaviour, IMovement, IPlayerPosition
{
    private Rigidbody2D body;
    [field: SerializeField] private float moveSpeed { get; set; }

    public IMovement.EDirection EDirectionOfMovement { get; set; }

    // Start is called before the first frame update
    public Vector2 PlayerPosition => gameObject.transform.position;
    public Vector2 PlayerFacing => new Vector2(animator.GetFloat("DirX"), animator.GetFloat("DirY"));
    private Vector2 MovementThisFrame { get; set; }
    private Vector2 movementThisFrame;
    private Func<Action> Move { get; set; }
    private Animator animator;


    private void Awake()
    {
        SetMovementThisFrame(Vector2.zero);
    }

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        if (FindObjectOfType<BattleV3.Battle>() is not null)
            ChangeAnimationLayer("Battle");
    }

    // Update is called once per frame
    void Update()
    {
  
    }

    private void FixedUpdate()
    {
        Move();
    }

    public void CallbackMove(InputAction.CallbackContext context)
    {
        var vec2 = context.ReadValue<Vector2>();

        SetMovementThisFrame(vec2);

        if (context.phase is InputActionPhase.Started or InputActionPhase.Performed)
            SetAnimationFacingThisFrame(vec2);

        animator.SetBool("Moving", IsMoving(vec2));

    }

    public void AIStopMoving()
    {
        SetMovementThisFrame(Vector2.zero);
    }

    public void AIMove(IMovement.EDirection eDirection)
    {
        switch (eDirection)
        {
            case IMovement.EDirection.Up:
                SetMovementThisFrame(Vector2.up);
                break;
            case IMovement.EDirection.Down:
                SetMovementThisFrame(Vector2.down);
                break;
            case IMovement.EDirection.Left:
                SetMovementThisFrame(Vector2.left);
                break;
            case IMovement.EDirection.Right:
                SetMovementThisFrame(Vector2.right);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(eDirection), eDirection, null);
        }
    }

    private void SetMovementThisFrame(Vector2 direction)
    {
        Move = () =>
        {
            MovementThisFrame = PlayerPosition + (direction * moveSpeed * Time.fixedDeltaTime);
            body.MovePosition(MovementThisFrame);
            return null;
        };
    }

    private void SetAnimationFacingThisFrame(Vector2 direction)
    {
        animator.SetFloat("DirX", direction.x);
        animator.SetFloat("DirY", direction.y);
    }

    public void SetAnimationFacing(Vector2 direction)
    {
        SetAnimationFacingThisFrame(direction);
    }

    public void PlayBattleAnimation(string animationName)
    {
        animator.Play("BattleAttackSeqLeft");
    }

   

public void SetAnimationFacing(IMovement.EDirection eDirection)
    {
        switch(eDirection)
        {
            case IMovement.EDirection.Up:
                SetAnimationFacingThisFrame(Vector2.up);
                break;
            case IMovement.EDirection.Down:
                SetAnimationFacingThisFrame(Vector2.down);
                break;
            case IMovement.EDirection.Left:
                SetAnimationFacingThisFrame(Vector2.left);
                break;
            case IMovement.EDirection.Right:
                SetAnimationFacingThisFrame(Vector2.right);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(eDirection), eDirection, null);
        }
    }

    private bool IsMoving(Vector2 direction)
    {
        return (direction.x != 0 || direction.y != 0) ? true : false;
    }

    public bool IsMoving()
    {
        return animator.GetBool("Moving");
    }

    public Vector2 MoveTo(Vector2 direction)
    {
        SetMovementThisFrame(direction);
     

        
        return PlayerPosition + (direction * moveSpeed * Time.fixedDeltaTime);
    }

    public void ChangeAnimationLayer(string layerName)
    {
        animator.SetLayerWeight(animator.GetLayerIndex(layerName), 1f);
    }

    public void SetAnimationTrigger(string trigger)
    {
        animator.SetTrigger(trigger);
    }
    

}