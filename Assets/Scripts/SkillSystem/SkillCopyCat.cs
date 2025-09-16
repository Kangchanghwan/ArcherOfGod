using System.Collections;
using UnityEngine;

public class SkillCopyCat : SkillBase
{
    [Header("Skill Settings")]
    [SerializeField] private GameObject cloningObject;
    
    [SerializeField] private int hp;
    [SerializeField] private float duration;
    
    
    public override string GetAnimationName() => "SkillCasting";
    public override IEnumerator SkillCoroutine()
    {
        yield return new WaitForSeconds(1f);
        Instantiate(cloningObject, transform.position, Quaternion.identity);
    }

}