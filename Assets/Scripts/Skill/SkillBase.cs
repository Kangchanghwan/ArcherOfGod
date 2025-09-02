using System;
using UnityEngine;

public class SkillBase : MonoBehaviour
{

    [Header("General detail")] [SerializeField]
    private float cooldown;
    [SerializeField]
    private float firstCooldown;
    [SerializeField]
    private float lastTimeUsed = 0f;
    
    
    protected void Awake()
    {
        lastTimeUsed = firstCooldown;
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

}
