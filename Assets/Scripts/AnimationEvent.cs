using System.StateSystem;
using Controller.Entity;
using UnityEngine;

public class AnimationEvent : MonoBehaviour
{
    private StateMachine _stateMachine;
    [SerializeField]
    private SkillJumpShoot skillJumpShoot;

    private void Awake()
    {
        _stateMachine = GetComponent<PlayerController>().StateMachine;
    }

    public void AnimationTrigger() => _stateMachine.TriggerCalled();
    public void OnJumpReadStartTrigger_JumpShoot() => skillJumpShoot.OnJumpReadyTrigger();
    public void OnJumpStart_JumpShoot() => skillJumpShoot.OnJumpStart();
    public void OnJumpEnd_JumpShoot() => skillJumpShoot.OnJumpEnd();
    public void OnHoverStart_JumpShoot() => skillJumpShoot.OnHoverStart();
    public void OnHoverEnd_JumpShoot() => skillJumpShoot.OnHoverEnd();
    public void OnShootArrow_JumpShoot() => skillJumpShoot.OnShootArrowTrigger();
    public void OnDownStart_JumpShoot() => skillJumpShoot.OnDownStart();
    public void OnDownEnd_JumpShoot() => skillJumpShoot.OnDownEnd();
    public void OnLeftStart_JumpShoot() => skillJumpShoot.OnLeftStart();
    public void OnLeftEnd_JumpShoot() => skillJumpShoot.OnLeftEnd();
}