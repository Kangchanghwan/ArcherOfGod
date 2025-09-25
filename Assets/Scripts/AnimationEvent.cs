using Component.Skill;
using Component.SkillSystem;
using Controller.Entity;
using UnityEngine;

public class AnimationEvent : MonoBehaviour
{
    [SerializeField]
    private SkillJumpShoot skillJumpShoot;
    private EntityBase _entity;

    private void Start()
    {
        _entity = GetComponent<EntityBase>();
        Debug.Assert(_entity != null);
    }

    public void AnimationTrigger() => _entity.AnimationTrigger();
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