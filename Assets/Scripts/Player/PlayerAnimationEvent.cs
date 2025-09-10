using UnityEngine;

public class PlayerAnimationEvent : MonoBehaviour
{
    private Player _player;

    private void Awake()
    {
        _player = GetComponentInParent<Player>();
    }

    public void AnimationTrigger() => _player.AnimationTrigger();

    public void OnJumpReadStartTrigger_JumpShoot() => _player.SkillState.SkillJumpShoot.OnJumpReadyTrigger();
    public void OnJumpStart_JumpShoot() => _player.SkillState.SkillJumpShoot.OnJumpStart();
    public void OnJumpEnd_JumpShoot() => _player.SkillState.SkillJumpShoot.OnJumpEnd();
    public void OnHoverStart_JumpShoot() => _player.SkillState.SkillJumpShoot.OnHoverStart();
    public void OnHoverEnd_JumpShoot() => _player.SkillState.SkillJumpShoot.OnHoverEnd();
    public void OnShootArrow_JumpShoot() => _player.SkillState.SkillJumpShoot.OnShootArrowTrigger();
    public void OnDownStart_JumpShoot() => _player.SkillState.SkillJumpShoot.OnDownStart();
    public void OnDownEnd_JumpShoot() => _player.SkillState.SkillJumpShoot.OnDownEnd();
    public void OnLeftStart_JumpShoot() => _player.SkillState.SkillJumpShoot.OnLeftStart();
    public void OnLeftEnd_JumpShoot() => _player.SkillState.SkillJumpShoot.OnLeftEnd();
}