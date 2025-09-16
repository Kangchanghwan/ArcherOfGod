using System;
using UnityEngine;

public abstract class EntityBase : MonoBehaviour
{

    public Rigidbody2D Rigidbody2D { get; private set; }
    public Animator Animator { get; private set; }
    
    protected bool FacingTarget { get; set; }
    public Transform Target => SetTarget();
    protected virtual void Awake()
    {
        Animator = GetComponentInChildren<Animator>();
        Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    public void FlipController(float x = 0)
    {
        if (x >= 0 && !FacingTarget)
        {
            Flip();
        }
        else if (x <= 0 && FacingTarget)
        {
            Flip();
        }
    }
    public void FaceTarget()
    {
        if (Target != null)
        {
            Vector3 directionToEnemy = Target.position - transform.position;
            if (directionToEnemy.x > 0 && !FacingTarget)
                Flip();
            else if (directionToEnemy.x < 0 && FacingTarget)
                Flip();
        }
    }
    private void Flip()
    {
        FacingTarget = !FacingTarget;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
    protected abstract Transform SetTarget();
}