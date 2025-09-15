using UnityEngine;

public interface IMovement
{
    public void Initialize(Rigidbody2D rb);
    public float Movement();
    public bool OnMove();
}