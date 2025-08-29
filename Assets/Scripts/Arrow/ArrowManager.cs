using UnityEngine;
using System.Collections;

public class ArrowManager : MonoBehaviour
{
    [Header("Flight Settings")]
    [SerializeField] private float minFlightTime = 0.5f;
    [SerializeField] private float maxFlightTime = 2f;
    [SerializeField] private float maxRange = 15f; // 최대 사거리
    [SerializeField] private float arcHeightMin = 2f; // 최소 호 높이
    [SerializeField] private float arcHeightMax = 5f; // 최대 호 높이
    
    [Header("References")]
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private GameObject shootEffect;
    
    [SerializeField] private Transform firePoint;
    [SerializeField] private Transform linearFirePoint;
    [SerializeField] private Transform targetPoint;
    
    [SerializeField] private LayerMask groundLayerMask = 1; // Ground 레이어
    [SerializeField] private float maxGroundCheckDistance = 50f;
    
    public void LinearShoot()
    {
        ShootEffectFX(firePoint.position);
       
        GameObject arrow = Instantiate(arrowPrefab, linearFirePoint.position, Quaternion.identity);
        
        LinearArrow arrowScript = arrow.GetComponent<LinearArrow>();
        
        Vector2 direction = (targetPoint.position - linearFirePoint.position).normalized;
        
        arrowScript.Launch(direction, gameObject);
    }
    
    
    public void BezierShoot(bool faceRight)
    {
        if (targetPoint == null) return;
        
        Vector2 startPos = firePoint.position;
        Vector2 targetPos = targetPoint.position;
    
        // 타겟에서 땅까지의 거리 측정
        RaycastHit2D hit = Physics2D.Raycast(targetPos, Vector2.down, maxGroundCheckDistance, groundLayerMask);
    
        Vector2 endPos;
        if (hit.collider != null)
        {
            
            float distanceToGround = hit.distance;
            endPos = faceRight
                ? new Vector2(targetPos.x + distanceToGround, hit.point.y + 0.1f)
                : new Vector2(targetPos.x - distanceToGround, hit.point.y + 0.1f);
        }
        else
        {
            endPos = targetPos; // 땅을 못 찾으면 현재 위치
        }        
        
        float distance = Vector2.Distance(startPos, endPos);
        
        // 거리에 따라 비행시간과 호의 높이 조절
        float t = Mathf.Clamp01(distance / maxRange);
        float flightTime = Mathf.Lerp(minFlightTime, maxFlightTime, t);
        float arcHeight = Mathf.Lerp(arcHeightMin, arcHeightMax, t * t);
        
        // 베지어 곡선의 제어점 계산
        Vector2 controlPoint = CalculateControlPoint(startPos, endPos, arcHeight);
        ShootEffectFX(firePoint.position);
        
        Vector2 initialDirection = (controlPoint - startPos).normalized;
        Quaternion initialRotation = Quaternion.FromToRotation(Vector2.up, initialDirection);
    
        GameObject arrow = Instantiate(arrowPrefab, firePoint.position, initialRotation);
        
        BezierArrow arrowScript = arrow.GetComponent<BezierArrow>();
        
        if (arrowScript == null)
        {
            arrowScript = arrow.AddComponent<BezierArrow>();
        }
        
        arrowScript.Launch(startPos, controlPoint, endPos, flightTime, gameObject);
    }

    public void ShootEffectFX(Vector3 position)
    {
        // var effect = Instantiate(shootEffect,  position, Quaternion.identity);
        // Destroy(effect, 0.5f);
    }

    
    private Vector2 CalculateControlPoint(Vector2 start, Vector2 end, float arcHeight)
    {
        // 시작점과 끝점의 중간점
        Vector2 midPoint = (start + end) / 2f;
        
        // 중간점에서 위쪽으로 arcHeight만큼 올린 지점을 제어점으로 사용
        return midPoint + Vector2.up * arcHeight;
    }
    
    
    

}
