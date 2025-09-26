using Component.Skill;
using Component.SkillSystem;
using Controller.Entity;
using Cysharp.Threading.Tasks;
using Interface;
using UnityEngine;

namespace MVC.Controller.Enemy
{
    public class AttackState : EntityStateBase<EnemyController>
    {
        private float _attackTimer;
        private bool _hasAttacked;
        
        public AttackState(EnemyController entityControllerBase) : base(entityControllerBase)
        {
        }

        protected override string GetAnimationName() => "Attack";

        public override void Enter()
        {
            base.Enter();
            _attackTimer = 0f;
            _hasAttacked = false;
            Controller.AttackReady();
        }

        public override void Execute()
        {
            base.Execute();
        
            _attackTimer += Time.deltaTime;
        
            // 공격 타이밍에 도달하면 공격 실행
            if (!_hasAttacked && _attackTimer >= Controller.AttackDelay)
            {
                Controller.ExecuteAttack();
                _hasAttacked = true;
            }


            if (TriggerCalled)
            {
                if (Random.Range(0f, 1f) < 0.5f)
                    Controller.ChangeMoveState();
                else
                    Controller.ChangeCastingState();
            }

        }
    }
    
    public class CastingState : EntityStateBase<EnemyController>
    {
        public CastingState(EnemyController controller) : base(controller)
        {
        }
        
        protected override string GetAnimationName() => "Casting";
        
        public override void Execute()
        {
            base.Execute();
            if (Controller.IsOnMove) 
                Controller.ChangeMoveState();
            if (TriggerCalled)
                Controller.ChangeAttackState();
        }
    }

    public class MoveState : EntityStateBase<EnemyController>
    {
        public MoveState(EnemyController controller) : base(controller)
        {
        }

        protected override string GetAnimationName() => "Move";
   
        public override void Execute()
        {
            base.Execute();
            Controller.ExecuteMove();
            if (Controller.IsOnMove is false)
                Controller.ChangeCastingState();
        }
    }

    public class SkillState : EntityStateBase<EnemyController>
    {
        private readonly SkillBase _skill;
        
        public SkillState(EnemyController controller, SkillBase skill) : base(controller)
        {
            _skill = skill;
        }

        protected override string GetAnimationName() => _skill.AnimationName;

        public override void Enter()
        {
            base.Enter();
            Controller.HandleInputSystem(false);
            Controller.FaceTarget();
            _skill.ExecuteSkill().Forget();
        }

        public override void Execute()
        {
            Debug.Log($"TriggerCalled{TriggerCalled}");
            base.Execute();
            if(TriggerCalled)
                Controller.ChangeCastingState();
        }

        public override void Exit()
        {
            base.Exit();
            Controller.HandleInputSystem(true);
        }
    }
    // Dead State
    public class DeadState : EntityStateBase<EnemyController>
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

        public override void Execute()
        {
            base.Execute();
            // 죽음 상태에서는 아무것도 하지 않음
        }
    }

    // Idle State
    public class IdleState : EntityStateBase<EnemyController>
    {

        public IdleState(EnemyController controller) : base(controller)
        {
        }
        public override void Enter()
        {
            base.Enter();
            if (Controller.Rigidbody2D != null)
                Controller.Rigidbody2D.simulated = false;
            
        }
        protected override string GetAnimationName() => "Idle";
        public override void Exit()
        {
            base.Exit();
            Controller.Rigidbody2D.simulated = true;
        }

    }
}