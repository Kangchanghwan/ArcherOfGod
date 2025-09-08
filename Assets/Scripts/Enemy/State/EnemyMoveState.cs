using UnityEngine;

public class EnemyMoveState : EnemyState
{    
    private float _dir = 1f;
    [SerializeField]
    private LayerMask wallLayerMask;
    [SerializeField]
    private float wallCheckDistance = 0.7f; 
    [SerializeField]
    private Vector2 wallCheckOffset = new Vector2(0.5f, 0f);
    [SerializeField]
    private float moveStateTime;
    [SerializeField]
    public float moveSpeed;


    public override void Enter()
    {
        base.Enter();
        _dir = Random.Range(0, 2) == 0 ? -1f : 1f;
    }

    // public override void Update()
    // {
    //     base.Update();
    //     
    //     if (enemy.canMove == false) return;
    //
    //     // 벽 감지
    //     if (IsWallDetected())
    //     {
    //         FlipDirection();
    //     }
    //
    //     // enemy.SetVelocity(dir * enemy.moveSpeed * Time.deltaTime, rigidbody2D.linearVelocity.y);
    //     
    //     if (stateTimer < 0)
    //         stateMachine.ChangeState(enemy.CastingState);
    // }
    private bool IsWallDetected()
    {
        Vector2 rayOrigin = (Vector2)Enemy.transform.position + wallCheckOffset;
        Vector2 rayDirection = Vector2.right * _dir;
        
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDirection, wallCheckDistance, LayerMask.GetMask("Wall"));
        
        return hit.collider != null;
    }

    private void FlipDirection()
    {
        _dir *= -1f;
    }
    public override void Exit()
    {
        base.Exit();
    }
}
