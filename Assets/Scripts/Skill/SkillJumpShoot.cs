using UnityEngine;

public class SkillJumpShoot : SkillBase
{
    
    
    [Header("Simple Timed Jump")]
    [SerializeField] private float jumpHeight = 0f;
    [SerializeField] private float upSpeed = 50f;          // 올라가는 속도
    [SerializeField] private float downSpeed = 30f;        // 내려가는 속도
    [SerializeField] private Rigidbody2D rb;
    private bool isJumped = false;
    private Vector3 originalPosition;  // 원래 위치 저장
    
    
    public void UpManualPosition()
    {
        if (!isJumped)
        {
            originalPosition = transform.position;
            
            // 올라가기
            rb.linearVelocity = new Vector2(0, upSpeed);
            
            float timeToReachPeak = jumpHeight / upSpeed;
            Invoke(nameof(FixYPosition), timeToReachPeak);
            
            isJumped = true;
        }
    }
    public void SetVelocity(float xVelocity, float yVelocity)
    {
        rb.linearVelocity = new Vector2(xVelocity, yVelocity);
        // FlipController(xVelocity);
    }

    public void DownManualPosition()
    {
        if (isJumped)
        {
            // Y축 제약 해제
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            // 하강
            rb.linearVelocity = new Vector2(0, -downSpeed);
            
            float timeToFallDown = jumpHeight / downSpeed;
            Invoke(nameof(StopAtGround), timeToFallDown);
        }
    }
    
    private void FixYPosition()
    {
        rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        rb.linearVelocity = Vector2.zero;
    }
    
    private void StopAtGround()
    {
        // 원래 위치로 복귀
        transform.position = originalPosition;
        
        // 모든 제약 해제
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.linearVelocity = Vector2.zero;
        
        isJumped = false;
    }
    
}
