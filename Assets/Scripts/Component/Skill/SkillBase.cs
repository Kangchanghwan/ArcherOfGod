using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Manager;
using UnityEngine;

namespace Component.Skill
{
    public enum SkillType
    {
        BombShoot,
        JumpShoot,
        RepidFire,
        CopyCat,
        WhirlWind
    }

    public class SkillEventArgs : EventArgs
    {
        public SkillType SkillType { get; }
        public float? CooldownTime { get; }

        public SkillEventArgs(SkillType skillType, float? cooldownTime = null)
        {
            SkillType = skillType;
            CooldownTime = cooldownTime;
        }
    }

    public abstract class SkillBase : MonoBehaviour
    {
        [Header("General detail")] [SerializeField]
        private float cooldown;

        [SerializeField] private UI_SkillSlot skillSlot;

        public SkillType SkillType { get; protected set; }
        public string AnimationName { get; protected set; }

        private float _lastTimeUsed;

        protected Rigidbody2D Rigidbody2D;
        protected Transform Target;
        protected Animator Anim;

        protected CancellationTokenSource CurrentSkillCancellationToken;
        private readonly Queue<GameObject> _pools = new Queue<GameObject>();


        public virtual void Initialize(Rigidbody2D rigidbody, Animator anim)
        {
            this.Rigidbody2D = rigidbody;
            this.Anim = anim;
            SetSkillOnCooldown();
            skillSlot?.StartCooldown(cooldown);
        }

        public void SetTarget(Transform target) => Target = target;
        public bool CanUseSkill() => !OnCooldown();

        public void CancelSkill()
        {
            if (CurrentSkillCancellationToken != null)
            {
                CurrentSkillCancellationToken.Cancel();
                CurrentSkillCancellationToken.Dispose();
                CurrentSkillCancellationToken = null;

                if (Rigidbody2D != null)
                {
                    Rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
                    Rigidbody2D.linearVelocity = Vector2.zero;
                }
            }
        }

        // protected GameObject PoolObject(GameObject prefab)
        // {
        //     GameObject pool = null;
        //
        //     if (_pools.Count > 0)
        //     {
        //         for (int i = 0; i < _pools.Count; i++)
        //         {
        //             pool = _pools.Dequeue();
        //             if (pool.activeSelf == false)
        //             {
        //                 _pools.Enqueue(pool);
        //                 pool.SetActive(true);
        //                 return pool;
        //             }
        //
        //             _pools.Enqueue(pool);
        //         }
        //     }
        //
        //     pool = Instantiate(prefab, transform.position, Quaternion.Euler(0, 0, -180f),
        //         GameManager.Instance.transform);
        //     _pools.Enqueue(pool);
        //     pool.SetActive(true);
        //     return pool;
        // }

        private bool OnCooldown() => Time.time < _lastTimeUsed + cooldown;

        public void SetSkillOnCooldown()
        {
            _lastTimeUsed = Time.time;
            skillSlot?.StartCooldown(cooldown);
        }

        public abstract UniTask SkillTask(CancellationToken cancellationToken);

        public async UniTask ExecuteSkill()
        {
            Rigidbody2D.linearVelocity = Vector2.zero;
            SetSkillOnCooldown();


            CurrentSkillCancellationToken = new CancellationTokenSource();

            try
            {
                await SkillTask(CurrentSkillCancellationToken.Token);
            }
            catch (OperationCanceledException)
            {
            }
            finally
            {
                CurrentSkillCancellationToken?.Dispose();
                CurrentSkillCancellationToken = null;
            }
        }
    }
}