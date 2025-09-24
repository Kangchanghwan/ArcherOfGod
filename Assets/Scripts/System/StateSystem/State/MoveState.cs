using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class MoveState : StateBase
{
    private MovementBase _movementBase;
    public bool OnMove =>  _movementBase.IsOnMove();
    

    protected override string GetAnimationName() => "Move";

    protected override void Start()
    {
        base.Start();
        _movementBase = GetComponent<MovementBase>();
    }
    public override void Enter()
    {
        base.Enter();
        _movementBase.Initialize(Rigidbody2D);
    }
    
    public override void Execute()
    {
        var dir = _movementBase.OnMove();
        Entity.FlipController(dir);
    }
    

}