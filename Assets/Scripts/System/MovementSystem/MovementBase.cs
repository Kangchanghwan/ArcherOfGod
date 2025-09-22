using UnityEngine;

public abstract class MovementBase : MonoBehaviour
{
    protected Rigidbody2D Rigidbody2D;

    public abstract void Initialize(Rigidbody2D rb);
    public abstract float OnMove();
    public abstract bool IsOnMove();
}