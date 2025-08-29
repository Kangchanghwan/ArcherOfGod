using UnityEngine;

public class SkillJumpShoot : SkillBase
{
    
    
    [Header("Simple Timed Jump")]
    [SerializeField] private float jumpHeight = 0f;
    [SerializeField] private float upSpeed = 50f;          // 올라가는 속도
    [SerializeField] private float downSpeed = 30f;        // 내려가는 속도
    
    private bool isJumped = false;
    private Vector3 originalPosition;  // 원래 위치 저장
    
    
    public void UpManualPosition()
    {
        if (!isJumped)
        {
            originalPosition = transform.position;
            
            // 올라가기
            entity.SetVelocity(0, upSpeed);
            
            float timeToReachPeak = jumpHeight / upSpeed;
            Invoke(nameof(FixYPosition), timeToReachPeak);
            
            isJumped = true;
        }
    }

    public void DownManualPosition()
    {
        if (isJumped)
        {
            // Y축 제약 해제
            entity.rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            
            // 하강
            entity.SetVelocity(0, -downSpeed);
            
            float timeToFallDown = jumpHeight / downSpeed;
            Invoke(nameof(StopAtGround), timeToFallDown);
        }
    }
    
    private void FixYPosition()
    {
        entity.rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        entity.rb.linearVelocity = Vector2.zero;
    }
    
    private void StopAtGround()
    {
        // 원래 위치로 복귀
        transform.position = originalPosition;
        
        // 모든 제약 해제
        entity.rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        entity.rb.linearVelocity = Vector2.zero;
        
        isJumped = false;
    }
    
}
