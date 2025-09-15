using UnityEngine;

public class PlayerSkillState : PlayerState
{
    public bool OnSkill { get; private set; }
    public SkillBase CurrentSkill { get; set; }
    public SkillJumpShoot SkillJumpShoot { get; private set; }
    public SkillBombShoot SkillBombShoot { get; private set; }
    public SkillRipedFire SkillRipedFire { get; private set; }
    public SkillWhirlWind SkillWhirlWind { get; private set; }
    public SkillCopyCat SkillCopyCat { get; private set; }
    
    protected override string GetAnimationName() => CurrentSkill.GetAnimationName();
    
    protected override void Start()
    {
        base.Start();
        
        SkillJumpShoot = GetComponent<SkillJumpShoot>();
        SkillBombShoot = GetComponent<SkillBombShoot>();
        SkillRipedFire = GetComponent<SkillRipedFire>();
        SkillWhirlWind = GetComponent<SkillWhirlWind>();
        SkillCopyCat = GetComponent<SkillCopyCat>();
    }
    
    
    public bool TryUseSkill(int skillNumber)
    {
        SkillBase skill = skillNumber switch
        {
            1 => SkillJumpShoot,
            2 => SkillBombShoot,
            3 => SkillRipedFire,
            4 => SkillWhirlWind,
            5 => SkillCopyCat,
            _ => null
        };
    
        if (skill == null) return false;
        if (skill.CanUseSkill() == false) return false;
        if (OnSkill) return false;
    
        CurrentSkill = skill;
        return true;
    }

    
    public override void Enter()
    {
        base.Enter();
        OnSkill = true;
        
        Rigidbody2D.linearVelocity = Vector2.zero;
        CurrentSkill.Initialize(
            Rigidbody2D,
            Animator,
            GameManager.Instance.PlayerOfTarget.GetTransform()
        );
        CurrentSkill.SetSkillOnCooldown();

        Player.FlipController();

        StartCoroutine(CurrentSkill.SkillCoroutine());
    }


    public override void Exit()
    {
        base.Exit();
        OnSkill = false;
    }
}