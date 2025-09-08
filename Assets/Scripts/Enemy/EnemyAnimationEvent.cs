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
    // public void LinearArrowTrigger() => enemy.LinearShoot();
    // // public void UpManualPosition() => enemy.skillManager.jumpShoot.UpManualPosition();
    // // public void DownManualPosition() => enemy.skillManager.jumpShoot.DownManualPosition();
    // public void StartManualRotation() => enemy.ActivateManualRotation(true);
    // public void StopManualRotation() => enemy.ActivateManualRotation(false);
    
}
