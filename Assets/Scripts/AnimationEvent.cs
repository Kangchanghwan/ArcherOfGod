using Component.Skill;
using Component.SkillSystem;
using MVC.Controller;
using MVC.Controller.CopyCat;
using UnityEngine;

public class AnimationEvent : MonoBehaviour
{
    [SerializeField]
    private SkillJumpShoot skillJumpShoot;
    private EntityControllerBase _entityControllerBase;

    public void Init()
    {
        _entityControllerBase = GetComponent<EntityControllerBase>();
        Debug.Assert(_entityControllerBase != null);
    }

    public void AnimationTrigger() => _entityControllerBase.AnimationTrigger();
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