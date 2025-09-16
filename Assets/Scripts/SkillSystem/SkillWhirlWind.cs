using System.Collections;
using UnityEngine;

public class SkillWhirlWind : SkillBase
{
    
    [Header("Skill Settings")] [SerializeField]
    private GameObject whirlWind;

    public override string GetAnimationName() => "WhirlWind";
    
    public override IEnumerator SkillCoroutine()
    {
        var poolObject = PoolObject(whirlWind);
        poolObject.transform.position = new Vector2(transform.position.x, transform.position.y + 1f);
        poolObject.GetComponent<WhirlWind>()?.SetTarget(Target);
        
        yield return new WaitForSeconds(10f) ;
        poolObject.gameObject.SetActive(false);
    }
    
}
