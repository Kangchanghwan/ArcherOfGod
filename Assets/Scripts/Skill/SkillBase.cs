using System;
using UnityEngine;

public class SkillBase : MonoBehaviour
{

    [Header("General detail")] [SerializeField]
    private float cooldown;
    private float lastTimeUsed;
    public Entity entity { get ; private set; }

    protected void Awake()
    {
        lastTimeUsed -= cooldown;

        entity = GetComponentInParent<Entity>();
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
