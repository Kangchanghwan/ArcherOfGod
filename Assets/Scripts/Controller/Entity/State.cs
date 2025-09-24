using System.StateSystem.State;
using Controller.Entity;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Component.Entity
{
    public abstract class PlayerStateBase : IState
    {
        protected readonly PlayerController Controller;
        private readonly Animator _animator;

        protected PlayerStateBase(PlayerController controller)
        {
            Controller = controller;
            _animator = Controller.Animator;
        }

        public bool TriggerCalled { get; private set; }

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

    public class AttackState : PlayerStateBase
    {
        public AttackState(PlayerController controller) : base(controller)
        {
        }
        protected override string GetAnimationName() => "Attack";

        public override void Enter()
        {
            base.Enter();
            Controller.Attack().Forget();
        }

        public override void Execute()
        {
            base.Execute();
            if (Controller.IsOnMove) 
                Controller.ChangeMoveState();
            if (TriggerCalled)
                Controller.ChangeCastingState();
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
    
    public class CastingState : PlayerStateBase
    {
        public CastingState(PlayerController controller) : base(controller)
        {
        }
        protected override string GetAnimationName() => "Casting";
        public override void Execute()
        {
            base.Execute();
            if (TriggerCalled)
                Controller.ChangeAttackState();
        }
    }

    public class MoveState : PlayerStateBase
    {
        public MoveState(PlayerController controller) : base(controller)
        {
        }

        protected override string GetAnimationName() => "Move";
        public override void Enter()
        {
            base.Enter();
        }

        public override void Execute()
        {
            base.Execute();
            Controller.ExecuteMove();
            if (Controller.IsOnMove is false)
                Controller.ChangeCastingState();
        }
        public override void Exit()
        {
            base.Exit();
        }
    }
    
}