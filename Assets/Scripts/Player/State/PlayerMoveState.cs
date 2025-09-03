using UnityEngine;

public class PlayerMoveState : PlayerState
{
    
    public bool facingRight = false;
    public bool canMove = true; 
    public PlayerMoveState(PlayerContext context, string animBoolName) : base(context, animBoolName)
    {
    }

    public override void Update()
    {
        base.Update();

        if (canMove == false) return; 

        rigidbody2D.linearVelocity = new Vector2(player.xInput *  player.moveSpeed * Time.deltaTime, rigidbody2D.linearVelocity.y);
        FlipController(player.xInput);        
        if (Mathf.Abs(player.xInput) < 0.1f)
        {
            playerController.ChangeState(playerController.CastingState);
        }
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
        Vector3 scale = player.transform.localScale;
        scale.x *= -1;
        player.transform.localScale = scale;
    }

    
}
