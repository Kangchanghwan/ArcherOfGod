using System.Collections;
using UnityEngine;

public class SkillBombShoot : SkillBase
{
    [Header("Skill Settings")] [SerializeField]
    private Arrow arrow;

    [SerializeField] private int damage;
    [SerializeField] private float arrowSpeed;

    [SerializeField] private Vector2 fireOffset;
    [SerializeField] private int arrowCount;
    public override string GetAnimationName() => "Attack";

    public override IEnumerator SkillCoroutine()
    {
        Anim.SetFloat("AttackSpeed", 1f);   
        yield return new WaitForSeconds(0.63f);
        
        for (int i = 0; i < arrowCount; i++)
        {
            var arrowGo = PoolObject(arrow.gameObject).GetComponent<Arrow>();
            Arrow(arrowGo);
        }
        yield return new WaitForSeconds(.2f);
        
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