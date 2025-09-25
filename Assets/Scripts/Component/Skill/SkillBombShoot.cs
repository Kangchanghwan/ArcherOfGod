using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Component.Skill
{
    public class SkillBombShoot : SkillBase
    {
        [Header("Skill Settings")]
        [SerializeField] private Arrow arrow;
        [SerializeField] private int damage;
        [SerializeField] private float arrowSpeed;
        [SerializeField] private Vector2 fireOffset;
        [SerializeField] private int arrowCount;

        public override void Initialize(Rigidbody2D rigidbody, Animator anim, Transform target)
        {
            base.Initialize(rigidbody, anim, target);
            SkillType = SkillType.BombShoot;
            AnimationName = "Attack";
        }

        public override async UniTask SkillTask(CancellationToken cancellationToken)
        {
            if (Anim != null)
                Anim.SetFloat("AttackSpeed", 1f);

            await UniTask.Delay(System.TimeSpan.FromSeconds(0.63f), cancellationToken: cancellationToken);

            for (int i = 0; i < arrowCount; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var arrowGo = PoolObject(arrow.gameObject).GetComponent<Arrow>();
                FireArrow(arrowGo);
            }

            await UniTask.Delay(System.TimeSpan.FromSeconds(0.2f), cancellationToken: cancellationToken);
        }

        private void FireArrow(Arrow arrow)
        {
            arrow.gameObject.SetActive(true);
            Vector2 p0 = (Vector2)transform.position + fireOffset;
            Vector2 p1 = Vector2.up * 7f;
            Vector2 targetPosition = Target.transform.position;
            Vector2 p2 = new Vector2(targetPosition.x + UnityEngine.Random.Range(0f, 3f), targetPosition.y);
            arrow.duration = arrowSpeed;
            arrow.ShotArrow(p0, p1, p2);
        }
    }
}