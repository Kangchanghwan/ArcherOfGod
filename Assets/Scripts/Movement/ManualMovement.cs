using System;
using UnityEngine;

public class ManualMovement : MonoBehaviour, IMovement
{
    [SerializeField] private float moveSpeed;

    private Rigidbody2D _rb;
    private float _xInput;
    

    private void OnEnable()
    {
        InputManagerSingleton.Instance.InputManager.Controller.Move.performed +=
            ctx => _xInput = ctx.ReadValue<float>();
        InputManagerSingleton.Instance.InputManager.Controller.Move.canceled +=
            _ => _xInput = 0f;
    }
    public void Initialize(Rigidbody2D rb)
    {
        _rb = rb;
    }

    public float Movement()
    {
        var movement = new Vector2(_xInput * moveSpeed * Time.deltaTime, _rb.linearVelocity.y);
        _rb.MovePosition(_rb.position + movement);
        return _xInput;
    }

    public bool OnMove()
    {
        return Mathf.Abs(_xInput) > 0.1f;
    }
}