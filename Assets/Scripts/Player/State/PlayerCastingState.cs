using UnityEngine;

public class PlayerCastingState : PlayerState
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public PlayerCastingState(PlayerContext context, string animBoolName) : base(context, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Rigidbody2D.linearVelocity = Vector2.zero;
    }

    public override void Update()
    {
        base.Update();
        
        if (Controller.OnMove)
        {
            Controller.ChangeState(Controller.MoveState);
        }

        if (TriggerCalled)
        {
            Controller.ChangeState(Controller.AttackState);
        }
    }

}
