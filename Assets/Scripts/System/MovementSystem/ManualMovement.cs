using System;
using UnityEngine;

public class ManualMovement : MovementBase
{
    [SerializeField] private float moveSpeed;

    private float _xInput;


    private void OnEnable()
    {
        InputManagerSingleton.Instance.InputManager.Controller.Move.performed +=
            ctx => _xInput = ctx.ReadValue<float>();
        InputManagerSingleton.Instance.InputManager.Controller.Move.canceled +=
            _ => _xInput = 0f;
    }

    public override void Initialize(Rigidbody2D rb)
    {
        Rigidbody2D = rb;
    }

    public override float OnMove()
    {
        var movement = new Vector2(_xInput * moveSpeed * Time.deltaTime, Rigidbody2D.linearVelocity.y);
        Rigidbody2D.MovePosition(Rigidbody2D.position + movement);
        return _xInput;
    }

    public override bool IsOnMove()
    {
        return Mathf.Abs(_xInput) > 0.1f;
    }
}