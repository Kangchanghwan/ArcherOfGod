using UnityEngine;

public class PlayerAnimationEvent : MonoBehaviour
{
    private PlayerController _playerController;

    private void Awake()
    {
        _playerController = GetComponentInParent<PlayerController>();
    }

    public void AnimationTrigger() => _playerController.AnimationTrigger();
    public void ArrowTrigger() => _playerController.AttackState.Attack();
    // public void LinearArrowTrigger() => _playerController.AttackState.LinearShoot();
    // public void UpManualPosition() => player.skillManager.jumpShoot.UpManualPosition();
    // public void DownManualPosition() => player.skillManager.jumpShoot.DownManualPosition();
    // public void StartManualRotation() => player.ActivateManualRotation(true);
    // public void StopManualRotation() => player.ActivateManualRotation(false);
    
}
