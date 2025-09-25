using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Component.Skill
{
    public class SkillJumpShoot : SkillBase
    {
        [Header("Skill Settings")]
        [SerializeField] private Arrow arrowPrefab;
        [SerializeField] private float jumpHeight;
        [SerializeField] private int damage;
        [SerializeField] private float arrowSpeed;
        [SerializeField] private Vector2 fireOffset;

        private Vector3 _originalPosition;
        private bool _up;
        private bool _hover;
        private bool _down;
        private bool _left;


        public override void Initialize(Rigidbody2D rigidbody, Animator anim, Transform target)
        {
            base.Initialize(rigidbody, anim, target);
            SkillType = SkillType.JumpShoot;
            AnimationName = "JumpShoot";
        }

        public override async UniTask SkillTask(CancellationToken cancellationToken)
        {
            _up = false;
            _hover = false;
            _down = false;
            _left = false;
            _originalPosition = Rigidbody2D.transform.position;

            await ExecuteJump(cancellationToken);
        }


        private async UniTask ExecuteJump(CancellationToken cancellationToken)
        {
            await UniTask.WaitUntil(() => _up, cancellationToken: cancellationToken);
            await RisePhase(cancellationToken);
            await UniTask.WaitUntil(() => _down, cancellationToken: cancellationToken);
            await FallPhase(cancellationToken);
        }

        private async UniTask RisePhase(CancellationToken cancellationToken)
        {
            if (Rigidbody2D != null)
            {
                Rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
                float peak = _originalPosition.y + jumpHeight;
                Rigidbody2D.transform.position = new Vector2(_originalPosition.x, peak);
            }

            await UniTask.WaitUntil(() => _hover, cancellationToken: cancellationToken);
        }

        private async UniTask FallPhase(CancellationToken cancellationToken)
        {
            if (Rigidbody2D != null)
            {
                Rigidbody2D.transform.position = new Vector2(_originalPosition.x, _originalPosition.y);
                Rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
                Rigidbody2D.linearVelocity = Vector2.zero;
            }

            await UniTask.WaitUntil(() => _left, cancellationToken: cancellationToken);
        }

        public void OnJumpReadyTrigger() => Debug.Log("OnJumpReadyTrigger");
        public void OnJumpStart() => _up = true;
        public void OnJumpEnd() => _up = false;
        public void OnHoverStart() => _hover = true;
        public void OnHoverEnd() => _hover = false;

        public void OnShootArrowTrigger() =>
            FireArrow(PoolObject(arrowPrefab.gameObject).GetComponent<Arrow>());

        public void OnDownStart() => _down = true;
        public void OnDownEnd() => _down = false;
        public void OnLeftStart() => _left = true;
        public void OnLeftEnd() => _left = false;

        private void FireArrow(Arrow arrow)
        {
            arrow.gameObject.SetActive(true);
            Vector2 p0 = (Vector2)transform.position + fireOffset;
            Vector2 p1 = p0;
            Vector2 p2 = Target.position;
            arrow.duration = arrowSpeed;
            arrow.ShotArrow(p0, p1, p2);
        }
    }
}