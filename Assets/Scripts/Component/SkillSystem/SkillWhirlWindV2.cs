namespace System.SkillSystem
{
    using UnityEngine;
    using Cysharp.Threading.Tasks;
    using System.Threading;

    public class SkillWhirlWindV2 : SkillBaseV2
    {
        [Header("Skill Settings")]
        [SerializeField] private GameObject whirlWind;

        public override void Initialize(Rigidbody2D rigidbody, Animator anim, Transform target)
        {
            base.Initialize(rigidbody, anim, target);
            SkillType = SkillType.WhirlWind;
            AnimationName = "WhirlWind";
        }

        public override async UniTask SkillTask(CancellationToken cancellationToken)
        {
            var poolObject = PoolObject(whirlWind);
            poolObject.transform.position = new Vector2(transform.position.x, transform.position.y + 1f);
            poolObject.GetComponent<WhirlWind>()?.SetTarget(Target);

            await UniTask.Delay(System.TimeSpan.FromSeconds(10f), cancellationToken: cancellationToken);
            poolObject.gameObject.SetActive(false);
        }
    }
}