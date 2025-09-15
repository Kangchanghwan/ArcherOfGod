using System;
using UnityEngine;

[RequireComponent(typeof(InputController))]
public class ManualMovement : MonoBehaviour, IMovement
{
    private Rigidbody2D _rb;
    private InputController _input;
    
    [SerializeField] private float moveSpeed;

    private void Awake()
    {
        _input = GetComponent<InputController>();
    }

    public void Initialize(Rigidbody2D rb)
    {
        _rb = rb;
    }

    public float Movement()
    {
        var movement = new Vector2(_input.XInput * moveSpeed * Time.deltaTime, _rb.linearVelocity.y);
        _rb.MovePosition(_rb.position + movement);
        return _input.XInput;
    }

    public bool OnMove()
    {
        return Mathf.Abs(_input.XInput) > 0.1f;
    }
}
