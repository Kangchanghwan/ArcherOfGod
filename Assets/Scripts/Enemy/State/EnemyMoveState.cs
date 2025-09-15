using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyMoveState : EnemyState
{
    private IMovement _movement;
    public bool OnMove =>  _movement.OnMove();
    

    protected override string GetAnimationName() => "Move";

    protected override void Start()
    {
        base.Start();
        _movement = GetComponent<IMovement>();
    }
    public override void Enter()
    {
        base.Enter();
        _movement.Initialize(Rigidbody2D);
    }
    
    public override void StateUpdate()
    {
        var dir = _movement.Movement();
        FlipController(dir);
    }
    
    // public override void Enter()
    // {
    //     base.Enter();
    //     Enemy.OnMove = true;
    //     _dir = Random.Range(0, 2) == 0 ? -1f : 1f;
    //     Enemy.FlipController(_dir);
    //     _moveStateTime = Random.Range(moveStateTimeMin, moveStateTimeMax);
    // }

    // public override void StateUpdate()
    // {
    //     _moveStateTime -= Time.fixedDeltaTime;
    //     if (_moveStateTime <= 0)
    //     {
    //         Enemy.OnMove = false;
    //         return;
    //     }
    //
    //     _movement.Movement(_dir);
    // }

}