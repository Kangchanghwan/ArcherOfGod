using UnityEngine;

public class PlayerAnimationEvent : MonoBehaviour
{
    private Player player;

    private void Awake()
    {
        player = GetComponentInParent<Player>();
    }

    public void AnimationTrigger() => player.AnimationTrigger();
    public void ArrowTrigger() => player.BezierShoot();
    public void LinearArrowTrigger() => player.LinearShoot();
    public void UpManualPosition() => player.skillManager.jumpShoot.UpManualPosition();
    public void DownManualPosition() => player.skillManager.jumpShoot.DownManualPosition();
    public void StartManualRotation() => player.ActivateManualRotation(true);
    public void StopManualRotation() => player.ActivateManualRotation(false);
    
}
