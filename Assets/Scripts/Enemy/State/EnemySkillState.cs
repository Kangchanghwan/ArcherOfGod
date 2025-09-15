using UnityEngine;

public class EnemySkillState : EnemyState
{
    private SkillBase _skill;
    public bool CanSkill => SkillJumpShoot.CanUseSkill();

    public SkillJumpShoot SkillJumpShoot { get; private set; }

    protected override void Start()
    {
        base.Start();
        SkillJumpShoot = GetComponent<SkillJumpShoot>();
    }
    protected override string GetAnimationName() => _skill.GetAnimationName();

    public override void Enter()
    {
        _skill = GetRandomSkill();
        base.Enter();
        Rigidbody2D.linearVelocity = Vector2.zero;
        _skill.Initialize(
            Rigidbody2D,
            Animator,
            GameManager.Instance.EnemyOfTarget.GetTransform()
        );
        _skill.SetSkillOnCooldown();
        Enemy.FlipController();
        StartCoroutine(_skill.SkillCoroutine());
    }

    public override void Exit()
    {
        base.Exit();
    }
    public SkillBase GetRandomSkill()
    {
        return SkillJumpShoot;
    }
}