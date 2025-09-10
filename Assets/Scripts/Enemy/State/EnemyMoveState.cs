using System.Collections;
using UnityEngine;

public class EnemyMoveState : EnemyState
{
    private float _dir = 1f;
    [SerializeField] private LayerMask wallLayerMask;
    [SerializeField] private float wallCheckDistance = 0.7f;
    [SerializeField] private Vector2 wallCheckOffset = new Vector2(0.5f, 0f);
    [SerializeField] private float moveStateTimeMin;
    [SerializeField] private float moveStateTimeMax;
    [SerializeField] public float moveSpeed;

    protected override string GetAnimationName() => "Move";

    public override void Enter()
    {
        base.Enter();
        Enemy.CanMove = true;
        _dir = Random.Range(0, 2) == 0 ? -1f : 1f;
        StartCoroutine(RandomMove());
    }

    private IEnumerator RandomMove()
    {
        float moveStateTime = Random.Range(moveStateTimeMin, moveStateTimeMax);

        while (moveStateTime > 0f)
        {
            moveStateTime -= Time.deltaTime;
            if (IsWallDetected())
            {
                FlipDirection();
            }

            OnMove();
            yield return null;
        }

        Enemy.CanMove = false;
    }

    private void OnMove()
    {
        var movement = new Vector2(_dir * moveSpeed * Time.deltaTime, Rigidbody2D.linearVelocity.y);
        Rigidbody2D.MovePosition(Rigidbody2D.position + movement);
        Enemy.FlipController(_dir);
    }

    private bool IsWallDetected()
    {
        Vector2 rayOrigin =
            (Vector2)Enemy.transform.position + wallCheckOffset;
        Vector2 rayDirection = Vector2.right * _dir;

        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDirection, wallCheckDistance, LayerMask.GetMask("Wall"));

        return hit.collider != null;
    }

    private void FlipDirection()
    {
        _dir *= -1f;
    }
}