using System.Collections.Generic;
using UnityEngine;

public class SkillState : StateBase
{
    // 공통 스킬 프로퍼티
    public bool OnSkill { get; private set; }
    private SkillBase _currentSkill;

    // 스킬 컴포넌트들 (동적으로 찾아서 사용)
    private SkillJumpShoot _skillJumpShoot;
    private SkillBombShoot _skillBombShoot;
    private SkillRipedFire _skillRipedFire;
    private SkillWhirlWind _skillWhirlWind;
    private SkillCopyCat _skillCopyCat;


    protected override string GetAnimationName() => _currentSkill.GetAnimationName();

    protected override void Start()
    {
        base.Start();
        InitializeSkills();
    }

    private void InitializeSkills()
    {
        // 모든 스킬 컴포넌트 찾기
        _skillJumpShoot = GetComponent<SkillJumpShoot>();
        _skillBombShoot = GetComponent<SkillBombShoot>();
        _skillRipedFire = GetComponent<SkillRipedFire>();
        _skillWhirlWind = GetComponent<SkillWhirlWind>();
        _skillCopyCat = GetComponent<SkillCopyCat>();
    }


    public bool TryUseSkill(int skillNumber)
    {
        SkillBase skill = GetSkillByNumber(skillNumber);

        if (skill == null) return false;
        if (!skill.CanUseSkill()) return false;
        if (OnSkill) return false;

        _currentSkill = skill;
        return true;
    }


    public bool TryUseRandomSkill()
    {
        if (OnSkill) return false;

        SkillBase skill = GetRandomAvailableSkill();
        if (skill == null || !skill.CanUseSkill()) return false;

        _currentSkill = skill;
        return true;
    }

    private SkillBase GetRandomAvailableSkill()
    {
        var availableSkills = new List<SkillBase>();

        // 모든 스킬 컴포넌트를 확인하고 사용 가능한 것들만 수집
        if (_skillJumpShoot != null && _skillJumpShoot.CanUseSkill())
            availableSkills.Add(_skillJumpShoot);

        if (_skillBombShoot != null && _skillBombShoot.CanUseSkill())
            availableSkills.Add(_skillBombShoot);

        if (_skillRipedFire != null && _skillRipedFire.CanUseSkill())
            availableSkills.Add(_skillRipedFire);

        if (_skillWhirlWind != null && _skillWhirlWind.CanUseSkill())
            availableSkills.Add(_skillWhirlWind);

        if (_skillCopyCat != null && _skillCopyCat.CanUseSkill())
            availableSkills.Add(_skillCopyCat);

        if (availableSkills.Count == 0)
        {
            return null;
        }

        // 랜덤하게 하나 선택
        int randomIndex = Random.Range(0, availableSkills.Count);
        SkillBase selectedSkill = availableSkills[randomIndex];

        return selectedSkill;
    }

    private SkillBase GetSkillByNumber(int skillNumber)
    {
        return skillNumber switch
        {
            1 => _skillJumpShoot,
            2 => _skillBombShoot,
            3 => _skillRipedFire,
            4 => _skillWhirlWind,
            5 => _skillCopyCat,
            _ => null
        };
    }

    public override void Enter()
    {
        base.Enter();
        OnSkill = true;

        if (_currentSkill == null) return;

        Rigidbody2D.linearVelocity = Vector2.zero;

        Transform target = Entity.Target;
        _currentSkill.Initialize(Rigidbody2D, Animator, target);
        _currentSkill.SetSkillOnCooldown();
        Entity.FaceTarget();
        StartCoroutine(_currentSkill.SkillCoroutine());
    }

    public override void Exit()
    {
        base.Exit();
        OnSkill = false;
        _currentSkill = null;
    }
}