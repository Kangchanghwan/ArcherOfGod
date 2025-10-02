using Interface;
using UnityEngine;
using IState = Interface.IState;

namespace MVC.Controller
{
    public enum EntityType
    {
        Player,
        Enemy,
        CopyCat
    }
    public abstract class EntityStateBase : IState 
    {
        protected readonly EntityControllerBase Controller;
        private readonly Animator _animator;

        protected EntityStateBase(EntityControllerBase entityControllerBase)
        {
            Controller = entityControllerBase;
            _animator = entityControllerBase.Animator;
        }

        protected bool TriggerCalled { get; private set; }

        public virtual void Enter()
        {
            _animator.SetBool(GetAnimationName(), true);
            TriggerCalled = false;
        }

        public virtual void Execute()
        {
        }

        public virtual void Exit()
        {
            _animator.SetBool(GetAnimationName(), false);
        }

        public bool AnimationTrigger() => TriggerCalled = true;

        protected abstract string GetAnimationName();
    }
    public abstract class EntityControllerBase : MonoBehaviour, ICombatable
    {
        protected Transform Target;
        public Rigidbody2D Rigidbody2D;
        public Animator Animator;

        protected StateMachine StateMachine;

        private bool _facingTarget;

        public virtual void Init()
        {
            Animator = GetComponent<Animator>();
            Rigidbody2D = GetComponent<Rigidbody2D>();

            Debug.Assert(Animator != null);
            Debug.Assert(Rigidbody2D != null);
        }

        public void FlipController(float x = 0)
        {
            if (x >= 0 && !_facingTarget)
            {
                Flip();
            }
            else if (x <= 0 && _facingTarget)
            {
                Flip();
            }
        }

        public void FaceTarget()
        {
            if (Target != null)
            {
                Vector3 directionToEnemy = Target.position - transform.position;
                if (directionToEnemy.x > 0 && !_facingTarget)
                    Flip();
                else if (directionToEnemy.x < 0 && _facingTarget)
                    Flip();
            }
        }

        private void Flip()
        {
            _facingTarget = !_facingTarget;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }

        public void AnimationTrigger()
        {
            if (StateMachine?.CurrentState is EntityStateBase state)
                state.AnimationTrigger();
        }

        public abstract EntityType GetEntityType();
        public void SetTarget(Transform transform) => Target = transform;

        public abstract void TakeDamage(float damage);
        public abstract void TargetOnDead();

    }
}