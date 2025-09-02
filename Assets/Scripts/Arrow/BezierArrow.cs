// BezierArrow.cs - 베지어 곡선을 따라 움직이는 화살

using System;
using UnityEngine;
using System.Collections;

public class BezierArrow : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float damage = 10f;
    [SerializeField] private float lifeTime = 5f;
    private GameObject owner;
    private Vector2 startPos;
    private Vector2 controlPoint;
    private Vector2 endPos;
    private float flightTime;
    private bool hasHit = false;
    private bool isFlying = false;
    
    // 회전을 위한 이전 위치 저장
    private Vector2 lastPosition;
    
    public void Launch(Vector2 start, Vector2 control, Vector2 end, float time, GameObject owner)
    {
        startPos = start;
        controlPoint = control;
        endPos = end;
        flightTime = time;
        lastPosition = transform.position;
        isFlying = true;
        this.owner = owner;
        
        StartCoroutine(FlyAlongBezierCurve());
        
        // 자동 삭제
        Destroy(gameObject, lifeTime);
    }
    
    private IEnumerator FlyAlongBezierCurve()
    {
        float elapsedTime = 0f;
        
        while (elapsedTime < flightTime && !hasHit)
        {
            float t = elapsedTime / flightTime;
            
            // 베지어 곡선 상의 현재 위치 계산
            Vector2 currentPos = BezierCurve.Quadratic(startPos, controlPoint, endPos, t);
            transform.position = currentPos;
            
            // 화살이 움직이는 방향으로 회전
            if (!hasHit)
            {
                UpdateRotation(currentPos);
            }
            
            lastPosition = currentPos;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        // 최종 위치로 이동
        if (!hasHit)
        {
            transform.position = endPos;
            isFlying = false;
        }
    }
    
    private void UpdateRotation(Vector2 currentPos)
    {
        Vector2 direction = (currentPos - lastPosition).normalized;
        
        if (direction.magnitude > 0.01f)
        {
            // 화살이 위쪽을 기본으로 바라보는 경우
            transform.up = direction.normalized;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasHit) return;

        // var entity = other.GetComponent<Entity>();
        //
        // if (entity != null && owner != null && entity.gameObject.name != owner.name)
        // {
        //     HitTarget(entity);
        // }

        var ground =  other.GetComponent<Ground>();

        if (ground != null)
        {
            HitGround();
        }
    }


    // private void HitTarget(Entity entity)
    // {
    //     hasHit = true;
    //     isFlying = false;
    //
    //     entity.health.TakeDamage(damage);
    //     
    //     // 충돌 효과
    //     GetComponent<Collider2D>().enabled = false;
    //     // 잠깐 후 삭제
    //     Destroy(gameObject, 1f);
    // }

    private void HitGround()
    {
        hasHit = true;
        isFlying = false;
        
        GetComponent<Collider2D>().enabled = false;
        
        Debug.Log("Bezier Arrow hit ground!");
        
        // 땅에 박힌 후 삭제
        Destroy(gameObject, 1f);
    }
    
    // 현재 비행 상태 확인
    public bool IsFlying()
    {
        return isFlying && !hasHit;
    }
}
