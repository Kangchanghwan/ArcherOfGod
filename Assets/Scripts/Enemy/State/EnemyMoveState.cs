using UnityEngine;

public class EnemyMoveState : EnemyState
{
    private float dir = 1f;

    private LayerMask wallLayerMask;
    private float wallCheckDistance = 0.7f; 
    private Vector2 wallCheckOffset = new Vector2(0.5f, 0f);

    public EnemyMoveState(StateMachine stateMachine, string animBoolName, Enemy enemy)
        : base(stateMachine, animBoolName, enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = enemy.moveStateTime;
        dir = Random.Range(0, 2) == 0 ? -1f : 1f;
    }

    public override void Update()
    {
        base.Update();
        
        if (enemy.canMove == false) return;

        // 벽 감지
        if (IsWallDetected())
        {
            FlipDirection();
        }

        // enemy.SetVelocity(dir * enemy.moveSpeed * Time.deltaTime, rigidbody2D.linearVelocity.y);
        
        if (stateTimer < 0)
            stateMachine.ChangeState(enemy.castingState);
    }
    private bool IsWallDetected()
    {
        Vector2 rayOrigin = (Vector2)enemy.transform.position + wallCheckOffset;
        Vector2 rayDirection = Vector2.right * dir;
        
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDirection, wallCheckDistance, LayerMask.GetMask("Wall"));
        
        return hit.collider != null;
    }

    private void FlipDirection()
    {
        dir *= -1f;
    }
    public override void Exit()
    {
        base.Exit();
    }
}
