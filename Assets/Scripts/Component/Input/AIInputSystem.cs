using UnityEngine;

namespace Component.Input
{
    using Random = UnityEngine.Random;

public class AIInputSystem : MonoBehaviour
{
    [Header("AI Movement Settings")]
    [SerializeField] private float moveChangeInterval = 2f;
    [SerializeField] private float idleChance = 0.3f;
    [SerializeField] private LayerMask wallLayerMask;
    [SerializeField] private float wallCheckDistance = 0.7f;
    [SerializeField] private Vector2 wallCheckOffset = new Vector2(0.5f, 0f);

    private float _moveTimer;
    private float _currentMoveInput;
    private float _moveDirection = 1f;
    private bool _isMoving = true;

    // 이동 입력만 제공 (InputManager와 동일한 인터페이스)
    public float HorizontalInput => _currentMoveInput;

    private void Start()
    {
        _moveTimer = moveChangeInterval;
        // 시작 시 랜덤 방향 설정
        _moveDirection = Random.Range(0f, 1f) > 0.5f ? 1f : -1f;
    }

    private void Update()
    {
        UpdateMovementInput();
        _moveTimer -= Time.deltaTime;
    }

    private void UpdateMovementInput()
    {
        // 벽 감지 시 방향 전환
        if (IsWallDetected())
        {
            _moveDirection *= -1f;
            _moveTimer = moveChangeInterval; // 방향 전환 후 타이머 리셋
        }

        // 주기적으로 행동 변경
        if (_moveTimer <= 0f)
        {
            // 정지할지 이동할지 랜덤 결정
            if (Random.Range(0f, 1f) < idleChance)
            {
                // Idle 상태
                _isMoving = false;
                _currentMoveInput = 0f;
            }
            else
            {
                // 이동 상태
                _isMoving = true;
                _moveDirection = Random.Range(0f, 1f) > 0.5f ? 1f : -1f;
                _currentMoveInput = _moveDirection;
            }
            
            // 다음 변경까지의 시간 설정
            _moveTimer = Random.Range(moveChangeInterval * 0.5f, moveChangeInterval * 1.5f);
        }

        // 현재 이동 중이라면 방향 유지
        if (_isMoving)
        {
            _currentMoveInput = _moveDirection;
        }
    }

    private bool IsWallDetected()
    {
        Vector2 rayOrigin = (Vector2)transform.position + wallCheckOffset;
        Vector2 rayDirection = Vector2.right * _moveDirection;

        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDirection, wallCheckDistance, wallLayerMask);
        return hit.collider != null;
    }

    // 디버그용 시각화
    private void OnDrawGizmosSelected()
    {
        // 벽 체크 레이
        Gizmos.color = Color.blue;
        Vector3 rayOrigin = transform.position + (Vector3)wallCheckOffset;
        Gizmos.DrawRay(rayOrigin, Vector3.right * _moveDirection * wallCheckDistance);

        // 현재 상태 표시
        Gizmos.color = _isMoving ? Color.green : Color.red;
        Gizmos.DrawWireCube(transform.position + Vector3.up, Vector3.one * 0.5f);

        // 이동 방향 표시
        if (_isMoving)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(transform.position, Vector3.right * _moveDirection * 2f);
        }
    }
}
}