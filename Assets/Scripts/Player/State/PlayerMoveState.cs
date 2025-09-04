using UnityEngine;

public class PlayerMoveState : PlayerState
{

    private readonly float _moveSpeed = 1000f;
    
    public PlayerMoveState(PlayerContext context, string animBoolName) : base(context, animBoolName)
    {
    }

    public override void Update()
    {
        base.Update();
        if (Controller.CanMove == false) return;

        
        if (Controller.OnMove)
        {
            OnMove();
        }
        else
        {
            Controller.ChangeState(Controller.CastingState);
        }
    }

    private void OnMove()
    {
        Rigidbody2D.linearVelocity =
            new Vector2(Controller.Input * _moveSpeed * Time.deltaTime, Rigidbody2D.linearVelocity.y);
        Controller.FlipController(Controller.Input);
    }
    
}