using System;
using System.Collections;
using UnityEngine;

public abstract class SkillBase : MonoBehaviour
{

    [Header("General detail")] [SerializeField]
    private float cooldown;
    [SerializeField]
    private float firstCooldown;
    [SerializeField]
    private float lastTimeUsed = 0f;
    [SerializeField]
    private string animationName;
    public string AnimationName => animationName;
    
    protected Rigidbody2D rb;
    protected Animator animator;

    public Rigidbody2D TargetRigidBody2D { get; set; }
    
    protected void Awake()
    {
        lastTimeUsed = firstCooldown;
    }
    
    public virtual void Initialize(IContextBase context)
    {
        animator = context.Animator;
        rb = context.RigidBody2D;
    }


    public bool CanUseSkill()
    {
        if (OnCooldown())
        {
            return false;
        }
        return true;
    }
    
    
    private bool OnCooldown() => Time.time < lastTimeUsed + cooldown;
    public void SetSillOnCooldown() => lastTimeUsed = Time.time;
    public void ResetCoolDownBy(float cooldownReduction) => lastTimeUsed = lastTimeUsed + cooldownReduction;
    public void ResetCooldown() => lastTimeUsed = Time.time;

    public virtual void ExecuteSkill()
    {
        StartCoroutine(SkillCoroutine());
    }
    protected abstract IEnumerator SkillCoroutine();

}
