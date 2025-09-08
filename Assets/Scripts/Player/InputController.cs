using System;
using UnityEngine;

public class InputController : MonoBehaviour
{
    private InputManager _inputManager;
    private PlayerStateMachine _playerStateMachine;

    private float _xInput;
    private void Awake()
    {
        _playerStateMachine = GetComponent<PlayerStateMachine>();
    }

    private void OnEnable()
    {
        _inputManager = new InputManager();
        _inputManager.Enable();
        _inputManager.Player.Move.performed += ctx => _xInput = ctx.ReadValue<float>();
        _inputManager.Player.Move.canceled += _ => _xInput = 0f;

        _inputManager.Player.Skill_1.performed += _ => _playerStateMachine.TryUseSkill(0);
        _inputManager.Player.Skill_2.performed += _ => _playerStateMachine.TryUseSkill(1);
        _inputManager.Player.Skill_3.performed += _ => _playerStateMachine.TryUseSkill(2);
    }

    private void Update()
    {
        _playerStateMachine.UpdateInputX(_xInput);
    }

    private void OnDisable()
    {
        _inputManager.Disable();
    }

    private void OnPlayerDeath() => Player.OnPlayerDeath += _inputManager.Disable;
}