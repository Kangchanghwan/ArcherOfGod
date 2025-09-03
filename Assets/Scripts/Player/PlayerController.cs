using System;
using UnityEngine;

public class PlayerContext : IContextBase
{
    public Player Player { get; set; }
    public PlayerController PlayerController { get; set; }
    public Animator Animator { get; set; }
    public Rigidbody2D RigidBody2D { get; set; }
}


public class PlayerController : MonoBehaviour
{
    public PlayerState CurrentState { get; private set; }

    public PlayerAttackState AttackState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerCastingState CastingState { get; private set; }
    public PlayerJumpShootState JumpShootState { get; private set; }
    public PlayerDeadState DeadState { get; private set; }
    public PlayerIdleState IdleState { get; private set; }
    
    public bool facingRight = false;
    public bool canMove = true; 

    private void Awake()
    {
        var context = new PlayerContext
        {
            Player = GetComponent<Player>(),
            PlayerController = this,
            Animator = GetComponentInChildren<Animator>(),
            RigidBody2D = GetComponent<Rigidbody2D>(),
        };

        AttackState = new PlayerAttackState(context, "Attack", GetComponent<ArrowManager>());
        MoveState = new PlayerMoveState(context, "Move");
        CastingState = new PlayerCastingState(context, "Casting");
        JumpShootState = new PlayerJumpShootState(context, "JumpShoot", 10f);
        DeadState = new PlayerDeadState(context, "Dead");
        IdleState = new PlayerIdleState(context, "Idle");
    }


    private void Update()
    {
        CurrentState.Update();
    }

    public void Initialize(PlayerState startState)
    {
        CurrentState = startState;
        startState.Enter();
    }

    public void ChangeState(PlayerState newState)
    {
        CurrentState.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }


    protected void FlipController(float x)
    {
        if (x > 0 && !facingRight)
        {
            Flip();
        }
        else if (x < 0 && facingRight)
        {
            Flip();
        }
    }

    public void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }


    public void AnimationTrigger()
    {
        CurrentState.AnimationTrigger();
    }
}