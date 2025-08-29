using System;
using UnityEngine;

public class EnemySkillManager : MonoBehaviour
{
    public SkillJumpShoot jumpShoot{get; private set;}

    private void Awake()
    {
        jumpShoot = GetComponentInChildren<SkillJumpShoot>();
    }
}
