using UnityEngine;

public class EnemyAnimationEvent : MonoBehaviour
{
    private Enemy enemy;

    private void Awake()
    {
        enemy = GetComponentInParent<Enemy>();
    }

    public void AnimationTrigger() => enemy.AnimationTrigger();
    public void ArrowTrigger() => enemy.BezierShoot();
    public void LinearArrowTrigger() => enemy.LinearShoot();
    public void UpManualPosition() => enemy.skillManager.jumpShoot.UpManualPosition();
    public void DownManualPosition() => enemy.skillManager.jumpShoot.DownManualPosition();
    public void StartManualRotation() => enemy.ActivateManualRotation(true);
    public void StopManualRotation() => enemy.ActivateManualRotation(false);
    
}
