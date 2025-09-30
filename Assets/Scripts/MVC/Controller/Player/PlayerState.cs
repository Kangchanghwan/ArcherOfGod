using Component.Skill;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Scripting;

namespace MVC.Controller.Player
{
    public class AttackState : EntityStateBase
    {
        private float _attackTimer;
        private bool _hasAttacked;
        private readonly PlayerController _controller;
        public AttackState(PlayerController controller) : base(controller)
        {
            _controller = controller;
        }
        protected override string GetAnimationName() => "Attack";

        public override void Enter()
        {
            base.Enter();
            _attackTimer = 0f;
            _hasAttacked = false;
            _controller.PrepareAttack();
        }

        public override void Execute()
        {
            base.Execute();
        
            _attackTimer += Time.deltaTime;
        
            // 공격 타이밍에 도달하면 공격 실행
            if (!_hasAttacked && _attackTimer >= _controller.AttackDelay)
            {
                _controller.PerformAttack();
                _hasAttacked = true;
            }
        
            if (_controller.IsOnMove) 
                _controller.ChangeMoveState();
            
            if (TriggerCalled)
                _controller.ChangeCastingState();
        }

    }
    
    public class CastingState : EntityStateBase
    {
        private readonly PlayerController _controller;

        public CastingState(PlayerController controller) : base(controller)
        {
            _controller = controller;
        }
        protected override string GetAnimationName() => "Casting";
        public override void Execute()
        {
            base.Execute();
            if (_controller.IsOnMove) 
                _controller.ChangeMoveState();
            if (TriggerCalled)
                _controller.ChangeAttackState();
        }
    }
    public class MoveState : EntityStateBase
    {
        private readonly PlayerController _controller;
        public MoveState(PlayerController controller) : base(controller)
        {
            _controller = controller;
        }

        protected override string GetAnimationName() => "Move";
   
        public override void Execute()
        {
            base.Execute();
            _controller.ProcessMovement();
            if (_controller.IsOnMove is false)
                _controller.ChangeCastingState();
        }
    }
    public class SkillState : EntityStateBase
    {
        private readonly SkillBase _skill;
        private readonly PlayerController _controller;
        
        public SkillState(PlayerController controller, SkillBase skill) : base(controller)
        {
            _skill = skill;
            _controller = controller;
        }

        protected override string GetAnimationName() => _skill.AnimationName;

        public override void Enter()
        {
            base.Enter();
            _controller.HandleInputSystem(false);
            Controller.FaceTarget();
            _skill.ExecuteSkill().Forget();
        }

        public override void Execute()
        {
            base.Execute();
            if(TriggerCalled)
                _controller.ChangeCastingState();
        }

        public override void Exit()
        {
            base.Exit();
            _controller.HandleInputSystem(true);
        }
    }
    public class DeadState : EntityStateBase
    {
        public DeadState(PlayerController controller) : base(controller)
        {
        }

        protected override string GetAnimationName() => "Dead";

        public override void Enter()
        {
            base.Enter();
            if (Controller.Rigidbody2D != null)
                Controller.Rigidbody2D.simulated = false;
        }

    }

    public class IdleState : EntityStateBase
    {
        private readonly PlayerController _controller;

        public IdleState(PlayerController controller) : base(controller)
        {
            _controller = controller;
        }
        public override void Enter()
        {
            base.Enter();
            _controller.HandleInputSystem(false);
        }
        protected override string GetAnimationName() => "Idle";
        public override void Exit()
        {
            base.Exit();
            _controller.HandleInputSystem(true);
        }
    }
}