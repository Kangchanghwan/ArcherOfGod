using UnityEngine;

public class EnemyAnimationEvent : MonoBehaviour
{
    private Enemy _enemy;

    private void Awake()
    {
        _enemy = GetComponentInParent<Enemy>();
    }

    public void AnimationTrigger() => _enemy.AnimationTrigger();
    public void ArrowTrigger() => _enemy.AttackState.Attack();
    public void OnJumpReadStartTrigger_JumpShoot() => _enemy.SkillJumpShoot.OnJumpReadyTrigger();
    public void OnJumpStart_JumpShoot() => _enemy.SkillJumpShoot.OnJumpStart();
    public void OnJumpEnd_JumpShoot() => _enemy.SkillJumpShoot.OnJumpEnd();
    public void OnHoverStart_JumpShoot() => _enemy.SkillJumpShoot.OnHoverStart();
    public void OnHoverEnd_JumpShoot() => _enemy.SkillJumpShoot.OnHoverEnd();
    public void OnShootArrow_JumpShoot() => _enemy.SkillJumpShoot.OnShootArrowTrigger();
    public void OnDownStart_JumpShoot() => _enemy.SkillJumpShoot.OnDownStart();
    public void OnDownEnd_JumpShoot() => _enemy.SkillJumpShoot.OnDownEnd();
    public void OnLeftStart_JumpShoot() => _enemy.SkillJumpShoot.OnLeftStart();
    public void OnLeftEnd_JumpShoot() => _enemy.SkillJumpShoot.OnLeftEnd();


}
