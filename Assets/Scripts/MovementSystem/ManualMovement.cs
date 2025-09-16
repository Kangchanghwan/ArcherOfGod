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
        Rb = rb;
    }

    public override float Movement()
    {
        var movement = new Vector2(_xInput * moveSpeed * Time.deltaTime, Rb.linearVelocity.y);
        Rb.MovePosition(Rb.position + movement);
        return _xInput;
    }

    public override bool OnMove()
    {
        return Mathf.Abs(_xInput) > 0.1f;
    }
}