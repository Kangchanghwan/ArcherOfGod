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

    public abstract class SkillBase : MonoBehaviour
    {
        [Header("General detail")] [SerializeField]
        private float cooldown;

        [SerializeField] private UI_SkillSlot skillSlot;

        public virtual SkillType SkillType { get; }
        public virtual string AnimationName { get; }

        private float _lastTimeUsed;

        protected Rigidbody2D Rigidbody2D;
        protected Transform Target;
        protected Animator Anim;

        protected CancellationTokenSource CurrentSkillCancellationToken;


        public void Initialize(Rigidbody2D rigidbody, Animator anim)
        {
            this.Rigidbody2D = rigidbody;
            this.Anim = anim;
            SetSkillOnCooldown();
            skillSlot?.StartCooldown(cooldown);
        }

        public void SetTarget(Transform target) => Target = target;
        public bool CanUseSkill() => !OnCooldown();
        

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