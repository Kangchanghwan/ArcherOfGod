using UnityEngine;
using System.Collections;

public class SkillJumpShoot : SkillBase
{
    [Header("Jump Settings")]
    [SerializeField] private float jumpHeight = 5f;
    [SerializeField] private float jumpDuration = 1f;      // 전체 점프 지속시간
    [SerializeField] private AnimationCurve jumpCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private bool _isSkillActive = false;
    private Vector3 _originalPosition;
    private Coroutine _currentSkillCoroutine;
    
    
    protected override IEnumerator SkillCoroutine()
    {
        _isSkillActive = true;
        _originalPosition = rb.transform.position;
        
        // 애니메이션 이벤트에서 호출될 때까지 대기하거나
        // 또는 바로 점프 시작
        print("SkillCoroutine() 실행");
        
        yield return StartCoroutine(ExecuteJump());
        // 스킬 완료 후 정리
        _isSkillActive = false;
    }

    private IEnumerator ExecuteJump()
    {
        // 상승 단계
        yield return StartCoroutine(RisePhase());
        
        // 공중에서 정지 (애니메이션 이벤트로 발사 타이밍 제어)
        yield return StartCoroutine(HoverPhase());
        
        // 하강 단계
        yield return StartCoroutine(FallPhase());
    }

    private IEnumerator RisePhase()
    {
        float elapsedTime = 0f;
        float riseDuration = jumpDuration * 0.4f; // 전체의 40%
        Vector3 startPos = _originalPosition;
        Vector3 peakPos = _originalPosition + Vector3.up * jumpHeight;

        // 물리 비활성화
        rb.bodyType = RigidbodyType2D.Kinematic;
        
        while (elapsedTime < riseDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / riseDuration;
            
            // 커브를 적용한 자연스러운 상승
            float curveValue = jumpCurve.Evaluate(t);
            rb.transform.position = Vector3.Lerp(startPos, peakPos, curveValue);
            
            yield return null;
        }
        
        rb.transform.position = peakPos;
    }

    private IEnumerator HoverPhase()
    {
        float hoverDuration = jumpDuration * 0.2f; // 전체의 20%
        float elapsedTime = 0f;
        
        // 공중에서 잠시 정지
        while (elapsedTime < hoverDuration)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator FallPhase()
    {
        float elapsedTime = 0f;
        float fallDuration = jumpDuration * 0.4f; // 전체의 40%
        Vector3 startPos = rb.transform.position;
        Vector3 endPos = _originalPosition;

        while (elapsedTime < fallDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / fallDuration;
            
            // 하강 커브 (더 빠르게 떨어지도록)
            float curveValue = Mathf.Pow(t, 2); // 가속하며 떨어짐
            rb.transform.position = Vector3.Lerp(startPos, endPos, curveValue);
            
            yield return null;
        }
        
        // 정확히 원래 위치로 복귀
        rb.transform.position = _originalPosition;
        
        // 물리 복구
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.linearVelocity = Vector2.zero;
    }
    
    // 스킬 취소
    public void CancelSkill()
    {
        if (_currentSkillCoroutine != null)
        {
            StopCoroutine(_currentSkillCoroutine);
            
            // 즉시 원래 상태로 복구
            rb.transform.position = _originalPosition;
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.linearVelocity = Vector2.zero;
            
            _isSkillActive = false;
        }
    }

    // 애니메이션 이벤트에서 호출 (점프 시작 시점)
    public void OnJumpStart()
    {
        Debug.Log("Jump Started!");
        // 필요시 추가 로직
        // StartCoroutine(RisePhase());
    }

    // 애니메이션 이벤트에서 호출 (발사 시점)
    public void OnShootArrow()
    {
        Debug.Log("Arrow Shot!");
        // 화살 발사 로직
        // ShootArrow();
        // StartCoroutine(HoverPhase());
        
    }

    // 애니메이션 이벤트에서 호출 (착지 시점)
    public void OnLanding()
    {
        Debug.Log("Landed!");
        // 착지 이펙트나 사운드
        // StartCoroutine(FallPhase());
    }
    


}