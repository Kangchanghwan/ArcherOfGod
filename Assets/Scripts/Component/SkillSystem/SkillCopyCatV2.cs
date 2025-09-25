namespace System.SkillSystem
{
    using UnityEngine;
    using Cysharp.Threading.Tasks;
    using System.Threading;

    public class SkillCopyCatV2 : SkillBaseV2
    {
        [Header("Skill Settings")]
        [SerializeField] private GameObject cloningObject;
        [SerializeField] private int hp;
        [SerializeField] private float duration;

        public override void Initialize(Rigidbody2D rigidbody, Animator anim, Transform target)
        {
            base.Initialize(rigidbody, anim, target);
            SkillType = SkillType.CopyCat;
            AnimationName = "SkillCasting";
        }
        public override async UniTask SkillTask(CancellationToken cancellationToken)
        {
            await UniTask.Delay(System.TimeSpan.FromSeconds(1f), cancellationToken: cancellationToken);
            Instantiate(cloningObject, transform.position, Quaternion.identity);
        }
    }
}