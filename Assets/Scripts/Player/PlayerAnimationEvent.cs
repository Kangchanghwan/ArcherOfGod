using UnityEngine;

public class PlayerAnimationEvent : MonoBehaviour
{
    private PlayerController _playerController;
    private SkillJumpShoot _skillJumpShoot;
    
    private void Awake()
    {
        _playerController = GetComponentInParent<PlayerController>();
        _skillJumpShoot = transform.parent.GetComponentInChildren<SkillJumpShoot>();
    }

    public void AnimationTrigger() => _playerController.AnimationTrigger();
    public void ArrowTrigger() => _playerController.AttackState.Attack();

    public void OnJumpStart_JumpShoot() => _skillJumpShoot.OnJumpStart();
    public void OnShootArrow_JumpShoot() => _skillJumpShoot.OnShootArrow();
    public void OnLanding__JumpShoot() => _skillJumpShoot.OnLanding();
        

    // public void LinearArrowTrigger() => _playerController.AttackState.LinearShoot();
    // public void UpManualPosition() => player.skillManager.jumpShoot.UpManualPosition();
    // public void DownManualPosition() => player.skillManager.jumpShoot.DownManualPosition();
    // public void StartManualRotation() => player.ActivateManualRotation(true);
    // public void StopManualRotation() => player.ActivateManualRotation(false);

}
