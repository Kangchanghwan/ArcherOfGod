using UnityEngine;

public class PlayerSkillState : PlayerState
{
    public bool OnSkill { get; private set; }
    public bool TriggerSkill { get; private set; }
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
        SubscribeToInput();
        
        SkillJumpShoot = GetComponent<SkillJumpShoot>();
        SkillBombShoot = GetComponent<SkillBombShoot>();
        SkillRipedFire = GetComponent<SkillRipedFire>();
        SkillWhirlWind = GetComponent<SkillWhirlWind>();
        SkillCopyCat = GetComponent<SkillCopyCat>();
    }
    
    private void SubscribeToInput()
    {
        InputController.OnSkillTriggered += HandleSkillInput;
    }

    private void HandleSkillInput(int skillNumber)
    {
        // 스킬 상태에서만 입력 처리
        if (OnSkill)
        {
            return; // 이미 스킬 사용 중이면 무시
        }

        // 스킬 사용 시도
        TryUseSkill(skillNumber);
    }

    public void TryUseSkill(int skillNumber)
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
    
        if (skill == null) return;
        if (skill.CanUseSkill() == false) return;
        if (OnSkill) return;
    
        CurrentSkill = skill;
        TriggerSkill = true;
    }

    
    public override void Enter()
    {
        TriggerSkill = false;
        base.Enter();

        Rigidbody2D.linearVelocity = Vector2.zero;
        CurrentSkill.Initialize(
            Rigidbody2D,
            Animator,
            GameManager.Instance.PlayerOfTarget.GetTransform()
        );
        CurrentSkill.SetSkillOnCooldown();

        FlipController();

        StartCoroutine(CurrentSkill.SkillCoroutine());
    }


    public override void Exit()
    {
        base.Exit();
        OnSkill = false;
    }
}