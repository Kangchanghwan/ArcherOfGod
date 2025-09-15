using System.Collections;
using UnityEngine;

public class SkillRipedFire : SkillBase
{
        
    [Header("Skill Settings")] [SerializeField]
    private Arrow arrow;

    [SerializeField] private int damage;
    [SerializeField] private float arrowSpeed;
    
    [SerializeField] private int arrowCount = 5;
    [SerializeField] private Vector2 fireOffset = new  Vector2(0.4f, 0.4f);
    
    private readonly float _repeatStartTime = 0.3f; // 반복 시작 지점
    private readonly float _repeatEndTime = 0.7f;   // 반복 끝 지점

    public override string GetAnimationName() => "Attack";
    public override IEnumerator SkillCoroutine()
    {
        int currentArrowCount = 0;
        
        // 연속 발사 루프
        while (currentArrowCount < arrowCount)
        {
            Anim.Play("Attack", 0, _repeatStartTime);
            
            yield return new WaitForSeconds((_repeatEndTime - _repeatStartTime) * 0.5f);
            
            var arrowGo = PoolObject(arrow.gameObject).GetComponent<Arrow>();
            Arrow(arrowGo);
            currentArrowCount++;
            
            yield return new WaitForSeconds((_repeatEndTime - _repeatStartTime) * 0.5f);
        }
        
        yield return new WaitForSeconds(0.2f);
    }
    
    private void Arrow(Arrow arrow)
    {
        arrow.gameObject.SetActive(true);
        Vector2 p0 = (Vector2)transform.position + fireOffset;
        Vector2 p1 = Vector2.up * 7f;
        Vector2 targetPosition = Target.transform.position;
        Vector2 p2 = new Vector2(targetPosition.x +  Random.Range(0f,3f),  targetPosition.y);
        arrow.Duration = arrowSpeed;
        arrow.ShotArrow(p0, p1, p2);
    }

}
