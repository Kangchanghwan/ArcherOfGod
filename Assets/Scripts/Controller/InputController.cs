using System;
using UnityEngine;

public class InputController : MonoBehaviour
{
    private InputManager _inputManager;
    private PlayerStateService _playerStateService;

    private float _xInput;
    private void Awake()
    {
        _playerStateService = GetComponent<PlayerStateService>();
    }

    private void OnEnable()
    {
        _inputManager = new InputManager();
        _inputManager.Enable();
        _inputManager.Player.Move.performed += ctx => _xInput = ctx.ReadValue<float>();
        _inputManager.Player.Move.canceled += _ => _xInput = 0f;

        _inputManager.Player.Skill_1.performed += _ => _playerStateService.TryUseSkill(0);
        _inputManager.Player.Skill_2.performed += _ => _playerStateService.TryUseSkill(1);
        _inputManager.Player.Skill_3.performed += _ => _playerStateService.TryUseSkill(2);
        _inputManager.Player.Skill_4.performed += _ => _playerStateService.TryUseSkill(3);
        _inputManager.Player.Skill_5.performed += _ => _playerStateService.TryUseSkill(4);
    }

    private void Update()
    {
        _playerStateService.UpdateInputX(_xInput);
    }

    private void OnDisable()
    {
        _inputManager.Disable();
    }

    private void OnPlayerDeath() => Player.OnPlayerDeath += _inputManager.Disable;
}