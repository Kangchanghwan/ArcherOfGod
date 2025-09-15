using System;
using UnityEngine;

public class InputController : MonoBehaviour
{
    private InputManager _inputManager;

    public float XInput { get; private set;}

    public static event Action<int> OnSkillTriggered;



    private void OnEnable()
    {
        _inputManager = new InputManager();
        _inputManager.Enable();

        _inputManager.Player.Move.performed += ctx => XInput = ctx.ReadValue<float>();
        _inputManager.Player.Move.canceled += _ => XInput = 0f; ;
        
        SubscribeToPlayerDeath();
    }

    
    private void Update()
    {
        CheckSkillInputs();
    }

    private void CheckSkillInputs()
    {
        // WasPressedThisFrame 사용으로 중복 입력 완전 방지
        if (_inputManager.Player.Skill_1.WasPressedThisFrame())
            TriggerSkill(1);
        else if (_inputManager.Player.Skill_2.WasPressedThisFrame())
            TriggerSkill(2);
        else if (_inputManager.Player.Skill_3.WasPressedThisFrame())
            TriggerSkill(3);
        else if (_inputManager.Player.Skill_4.WasPressedThisFrame())
            TriggerSkill(4);
        else if (_inputManager.Player.Skill_5.WasPressedThisFrame())
            TriggerSkill(5);
    }

    private void TriggerSkill(int skillNumber)
    {
        OnSkillTriggered?.Invoke(skillNumber);
        Debug.Log($"스킬 {skillNumber} 트리거됨");
    }

    private void SubscribeToPlayerDeath() =>
        Player.OnPlayerDeath += HandlePlayerDeath;
    

    private void HandlePlayerDeath()
    {
        _inputManager?.Disable();
        Debug.Log("플레이어 사망으로 입력 비활성화");
    }

    private void OnDisable()
    {
        _inputManager?.Disable();
        Player.OnPlayerDeath -= HandlePlayerDeath;
    }

    private void OnDestroy()
    {
        OnSkillTriggered = null;
    }
}