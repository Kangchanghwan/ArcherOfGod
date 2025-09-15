using UnityEngine;

public class PlayerAnimationEvent : MonoBehaviour
{
    private PlayerStateMachine _stateMachine;

    private void Awake()
    {
        _stateMachine = GetComponentInParent<PlayerStateMachine>();
    }

    public void AnimationTrigger() => _stateMachine.AnimationTrigger();

    public void OnJumpReadStartTrigger_JumpShoot() => _stateMachine.SkillState.SkillJumpShoot.OnJumpReadyTrigger();
    public void OnJumpStart_JumpShoot() => _stateMachine.SkillState.SkillJumpShoot.OnJumpStart();
    public void OnJumpEnd_JumpShoot() => _stateMachine.SkillState.SkillJumpShoot.OnJumpEnd();
    public void OnHoverStart_JumpShoot() => _stateMachine.SkillState.SkillJumpShoot.OnHoverStart();
    public void OnHoverEnd_JumpShoot() => _stateMachine.SkillState.SkillJumpShoot.OnHoverEnd();
    public void OnShootArrow_JumpShoot() => _stateMachine.SkillState.SkillJumpShoot.OnShootArrowTrigger();
    public void OnDownStart_JumpShoot() => _stateMachine.SkillState.SkillJumpShoot.OnDownStart();
    public void OnDownEnd_JumpShoot() => _stateMachine.SkillState.SkillJumpShoot.OnDownEnd();
    public void OnLeftStart_JumpShoot() => _stateMachine.SkillState.SkillJumpShoot.OnLeftStart();
    public void OnLeftEnd_JumpShoot() => _stateMachine.SkillState.SkillJumpShoot.OnLeftEnd();
}