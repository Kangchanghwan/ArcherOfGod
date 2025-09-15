using Unity.Mathematics.Geometry;
using UnityEngine;

public class PlayerMoveState : PlayerState
{
    private IMovement _movement;
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
    public bool OnMove() => _movement.OnMove();

}