using UnityEngine;

public class PlayerSkillState : PlayerState
{
    public SkillBase CurrentSkill { get; set; }
    public SkillJumpShoot SkillJumpShoot { get; private set; }
    public SkillBombShoot SkillBombShoot { get; private set; }

    private void Start()
    {
        SkillJumpShoot = GetComponent<SkillJumpShoot>();
        SkillBombShoot = GetComponent<SkillBombShoot>();
    }
    protected override string GetAnimationName() => CurrentSkill.GetAnimationName();

    public override void Enter()
    {
        if (CurrentSkill == null) return;
        base.Enter();

        Rigidbody2D.linearVelocity = Vector2.zero;
        CurrentSkill.Initialize(
            Rigidbody2D,
            Animator,
            GameManager.Instance.PlayerOfTarget.GetTransform()
        );
        CurrentSkill.SetSkillOnCooldown();

        Player.CanMove = false;
        Player.CanSkill = false;
        Player.FlipController();

        StartCoroutine(CurrentSkill.SkillCoroutine());
    }


    public override void Exit()
    {
        base.Exit();
        Player.CanMove = true;
        Player.CanSkill = true;
    }
}