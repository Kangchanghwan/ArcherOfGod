using Unity.Mathematics.Geometry;
using UnityEngine;

public class PlayerMoveState : PlayerState
{

    [SerializeField]
    private float moveSpeed = 1000f;
    public float XInput { get; set; }
    

    public override void StateUpdate()
    {
        OnMove();
    }

    private void OnMove()
    {
        Rigidbody2D.linearVelocity =
            new Vector2(XInput * moveSpeed * Time.deltaTime, Rigidbody2D.linearVelocity.y);
        Player.FlipController(XInput);
    }
    
}