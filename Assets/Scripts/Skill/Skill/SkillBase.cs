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
    
    protected Coroutine currentSkillCoroutine;
    
    protected void Awake()
    {
        lastTimeUsed = firstCooldown;
    }
    
    public void Initialize(IContextBase context)
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
    // 스킬 취소
    public void CancelSkill()
    {
        if (currentSkillCoroutine != null)
        {
            StopCoroutine(currentSkillCoroutine);
            
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.linearVelocity = Vector2.zero;
        }
    }
    
    private bool OnCooldown() => Time.time < lastTimeUsed + cooldown;
    public void SetSkillOnCooldown() => lastTimeUsed = Time.time;
    public void ResetCoolDownBy(float cooldownReduction) => lastTimeUsed = lastTimeUsed + cooldownReduction;
    public void ResetCooldown() => lastTimeUsed = Time.time;
    
    public abstract IEnumerator SkillCoroutine();

}
