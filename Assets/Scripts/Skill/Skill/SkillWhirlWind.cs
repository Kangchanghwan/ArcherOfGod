using System.Collections;
using UnityEngine;

public class SkillWhirlWind : SkillBase
{
    
    [Header("Skill Settings")] [SerializeField]
    private GameObject whirlWind;
    public override string GetAnimationName() => "WhirlWind";
    
    public override IEnumerator SkillCoroutine()
    {
        var poolObject = PoolObject(this.whirlWind);
        poolObject.transform.position = new Vector2(transform.position.x, transform.position.y + 1f);
        yield return new WaitForSeconds(1f) ;
    }
    
}
