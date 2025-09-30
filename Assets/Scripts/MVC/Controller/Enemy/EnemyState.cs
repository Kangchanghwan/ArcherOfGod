using Component.Skill;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Scripting;

namespace MVC.Controller.Enemy
{
    public class AttackState : EntityStateBase
    {
        private float _attackTimer;
        private bool _hasAttacked;
        private readonly EnemyController _controller;
        
        public AttackState(EnemyController controller) : base(controller)
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


            if (TriggerCalled)
            {
                if (Random.Range(0f, 1f) < 0.5f)
                    _controller.ChangeMoveState();
                else
                    _controller.ChangeCastingState();
            }

        }
    }
    public class CastingState : EntityStateBase
    {
        private readonly EnemyController _controller;
        public CastingState(EnemyController controller) : base(controller)
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
        private readonly EnemyController _controller;

        public MoveState(EnemyController controller) : base(controller)
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
        private readonly EnemyController _controller;
        
        public SkillState(EnemyController controller, SkillBase skill) : base(controller)
        {
            _skill = skill;
            _controller = controller;
        }

        protected override string GetAnimationName() => _skill.AnimationName;

        public override void Enter()
        {
            base.Enter();
            _controller.HandleInputSystem(false);
            _controller.FaceTarget();
            _skill.ExecuteSkill().Forget();
        }

        public override void Execute()
        {
            Debug.Log($"TriggerCalled{TriggerCalled}");
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
        public DeadState(EnemyController controller) : base(controller)
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

        private readonly EnemyController _controller;
        
        public IdleState(EnemyController controller) : base(controller)
        {
            _controller = controller;
        }
        public override void Enter()
        {
            base.Enter();
            if (_controller.Rigidbody2D != null)
                _controller.Rigidbody2D.simulated = false;
            
        }
        protected override string GetAnimationName() => "Idle";
        public override void Exit()
        {
            base.Exit();
            _controller.Rigidbody2D.simulated = true;
        }

    }
}