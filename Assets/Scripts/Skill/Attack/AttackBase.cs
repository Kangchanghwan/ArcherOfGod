using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackBase : MonoBehaviour
{
    protected Animator animator;
    protected Rigidbody2D rigidbody2D;
    public Rigidbody2D TargetRigidBody2D { get; set; }

    public virtual void Initialize(IContextBase context)
    {
        animator = context.Animator;
        rigidbody2D = context.RigidBody2D;
    }

    public abstract void Attack();
}
