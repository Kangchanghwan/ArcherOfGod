using System;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// InputManager의 싱글톤 래퍼 클래스
/// 전역적으로 입력을 관리합니다.
/// </summary>
public class InputManagerSingleton
{
    private static InputManagerSingleton _instance;

    private readonly InputManager _inputManager;
    public InputManager InputManager => _inputManager;


    public static InputManagerSingleton Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new InputManagerSingleton(); // 락 없이
            }

            return _instance;
        }
    }


    // private 생성자 - 외부에서 직접 생성 불가
    private InputManagerSingleton()
    {
        _inputManager = new InputManager();
    }

    /// <summary>
    /// 입력 시스템 활성화
    /// </summary>
    public void Enable()
    {
        _inputManager?.Enable();
    }

    /// <summary>
    /// 입력 시스템 비활성화
    /// </summary>
    public void Disable()
    {
        _inputManager?.Disable();
    }
}