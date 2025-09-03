using UnityEngine;

public class PlayerCastingState : PlayerState
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public PlayerCastingState(PlayerContext context, string animBoolName) : base(context, animBoolName)
    {
    }

    public override void Update()
    {
        base.Update();
        
        if (Mathf.Abs(player.xInput) > 0.1f)
        {
            playerController.ChangeState(playerController.MoveState);
        }

        if (triggerCalled)
        {
            playerController.ChangeState(playerController.AttackState);
        }
    }

}
