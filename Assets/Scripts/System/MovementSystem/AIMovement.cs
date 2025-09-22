using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class AIMovement : MovementBase
{
    [SerializeField] private float speed;
    [SerializeField] private LayerMask wallLayerMask;
    [SerializeField] private float wallCheckDistance = 0.7f;
    [SerializeField] private Vector2 wallCheckOffset = new Vector2(0.5f, 0f);
    [SerializeField] private float moveStateTimeMin;
    [SerializeField] private float moveStateTimeMax;

    private float _dir = 1f;
    private float _moveStateTime;


    public override void Initialize(Rigidbody2D rb)
    {
        Rigidbody2D = rb;
        _moveStateTime = Random.Range(moveStateTimeMin, moveStateTimeMax);
    }

    public override float OnMove()
    {
        if (IsWallDetected())
        {
            FlipDirection();
        }

        var movement = new Vector2(_dir * speed * Time.deltaTime, Rigidbody2D.linearVelocity.y);
        Rigidbody2D.MovePosition(Rigidbody2D.position + movement);
        return _dir;
    }

    private void Update()
    {
        _moveStateTime -= Time.deltaTime;
    }

    public override bool IsOnMove()
    {
        return _moveStateTime > 0f;
    }

    private bool IsWallDetected()
    {
        Vector2 rayOrigin = (Vector2)transform.position + wallCheckOffset;
        Vector2 rayDirection = Vector2.right * _dir;

        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDirection, wallCheckDistance, LayerMask.GetMask("Wall"));

        return hit.collider != null;
    }

    private void FlipDirection()
    {
        _dir *= -1f;
    }
}