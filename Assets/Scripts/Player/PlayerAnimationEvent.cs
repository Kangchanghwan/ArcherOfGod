using UnityEngine;

public class PlayerAnimationEvent : MonoBehaviour
{
    private Player _player;
    
    private void Awake()
    {
        _player = GetComponentInParent<Player>();
    }

    public void AnimationTrigger() => _player.AnimationTrigger();
    public void ArrowTrigger() => _player.AttackState.Attack();


    public void OnJumpReadStartTrigger_JumpShoot() => _player.SkillJumpShoot.OnJumpReadyTrigger();
    public void OnJumpStart_JumpShoot() => _player.SkillJumpShoot.OnJumpStart();
    public void OnJumpEnd_JumpShoot() => _player.SkillJumpShoot.OnJumpEnd();
    public void OnHoverStart_JumpShoot() => _player.SkillJumpShoot.OnHoverStart();
    public void OnHoverEnd_JumpShoot() => _player.SkillJumpShoot.OnHoverEnd();
    public void OnShootArrow_JumpShoot() => _player.SkillJumpShoot.OnShootArrowTrigger();
    public void OnDownStart_JumpShoot() => _player.SkillJumpShoot.OnDownStart();
    public void OnDownEnd_JumpShoot() => _player.SkillJumpShoot.OnDownEnd();
    public void OnLeftStart_JumpShoot() => _player.SkillJumpShoot.OnLeftStart();
    public void OnLeftEnd_JumpShoot() => _player.SkillJumpShoot.OnLeftEnd();




    // public void OnJumpStart_JumpShoot() => _player.SkillJumpShoot.OnJumpStart();
    // public void OnShootArrow_JumpShoot() =>  _player.SkillJumpShoot.OnShootArrow();
    // public void OnLanding__JumpShoot() =>  _player.SkillJumpShoot.OnLanding();
    //
    //
    // public void LinearArrowTrigger() => _playerController.AttackState.LinearShoot();
    // public void UpManualPosition() => player.skillManager.jumpShoot.UpManualPosition();
    // public void DownManualPosition() => player.skillManager.jumpShoot.DownManualPosition();
    // public void StartManualRotation() => player.ActivateManualRotation(true);
    // public void StopManualRotation() => player.ActivateManualRotation(false);

}
