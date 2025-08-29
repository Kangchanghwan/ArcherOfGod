using System;
using UnityEngine;

public class PlayerSkillManager : MonoBehaviour
{
    public SkillJumpShoot jumpShoot{get; private set;}

    private void Awake()
    {
        jumpShoot = GetComponentInChildren<SkillJumpShoot>();
    }
}
