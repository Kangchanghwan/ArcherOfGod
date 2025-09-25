using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using IState = Interface.IState;
using StateMachine = Component.StateSystem.StateMachine;

namespace Controller.Entity
{
    public abstract class EntityStateBase<T> : IState where T : EntityControllerBase
    {
        protected readonly T Controller;
        private readonly Animator _animator;

        protected EntityStateBase(T entityControllerBase)
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

    public abstract class EntityControllerBase : MonoBehaviour
    {
        [SerializeField] public Transform Target;
        public Rigidbody2D Rigidbody2D;
        public Animator Animator;

        public StateMachine StateMachine { get; protected set; }

        private bool _facingTarget;

        protected virtual void Awake()
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

        public abstract void AnimationTrigger();
    }
}