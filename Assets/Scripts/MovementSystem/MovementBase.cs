using UnityEngine;

public abstract class MovementBase : MonoBehaviour
{
    protected Rigidbody2D Rb;

    public abstract void Initialize(Rigidbody2D rb);
    public abstract float Movement();
    public abstract bool OnMove();
}