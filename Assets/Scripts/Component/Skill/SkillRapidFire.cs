using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Component.Skill
{
    public class SkillRapidFire : SkillBase
    {
        [Header("Skill Settings")]
        [SerializeField] private Arrow arrow;
        [SerializeField] private int damage;
        [SerializeField] private float arrowSpeed;
        [SerializeField] private int arrowCount = 5;
        [SerializeField] private Vector2 fireOffset = new Vector2(0.4f, 0.4f);

        private readonly float _repeatStartTime = 0.3f;
        private readonly float _repeatEndTime = 0.7f;

        public override void Initialize(Rigidbody2D rigidbody, Animator anim)
        {
            base.Initialize(rigidbody, anim);
            SkillType = SkillType.RepidFire;
            AnimationName = "Attack";
        }

        public override async UniTask SkillTask(CancellationToken cancellationToken)
        {
            int currentArrowCount = 0;

            while (currentArrowCount < arrowCount)
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (Anim != null)
                    Anim.Play("Attack", 0, _repeatStartTime);

                await UniTask.Delay(System.TimeSpan.FromSeconds((_repeatEndTime - _repeatStartTime) * 0.5f),
                    cancellationToken: cancellationToken);

                var arrowGo = ObjectPool.Instance.GetObject(arrow.gameObject, transform).GetComponent<Arrow>();
                FireArrow(arrowGo);
                currentArrowCount++;

                await UniTask.Delay(System.TimeSpan.FromSeconds((_repeatEndTime - _repeatStartTime) * 0.5f),
                    cancellationToken: cancellationToken);
            }

            await UniTask.Delay(System.TimeSpan.FromSeconds(0.2f), cancellationToken: cancellationToken);
        }

        private void FireArrow(Arrow arrow)
        {
            Vector2 p0 = (Vector2)transform.position + fireOffset;
            Vector2 p1 = Vector2.up * 7f;
            Vector2 targetPosition = Target.transform.position;
            Vector2 p2 = new Vector2(targetPosition.x + UnityEngine.Random.Range(0f, 3f), targetPosition.y);
            arrow.duration = arrowSpeed;
            UniTask.FromResult(arrow.ShotArrow(p0, p1, p2));
        }
    }
}