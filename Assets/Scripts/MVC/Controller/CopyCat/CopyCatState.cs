using System.Threading;
using Component.Skill;
using Component.SkillSystem;
using Cysharp.Threading.Tasks;
using Interface;
using UnityEngine;

namespace MVC.Controller.CopyCat
{
    public abstract class CopyCatStateBase : IState
    {
        protected readonly CopyCatController Controller;
        protected readonly Animator Animator;

        protected CopyCatStateBase(CopyCatController controller)
        {
            Controller = controller;
            Animator = Controller.Animator;
        }

        public bool TriggerCalled { get; private set; }

        public virtual void Enter()
        {
            Animator.SetBool(GetAnimationName(), true);
            TriggerCalled = false;
        }
        
        public virtual void Execute()
        {
        }
    
        public virtual void Exit()
        {
            Animator.SetBool(GetAnimationName(), false);
        }

        public bool AnimationTrigger() => TriggerCalled = true;
    
        protected abstract string GetAnimationName();
    }

    // Attack State
    public class AttackState : CopyCatStateBase
    {
        private float _attackTimer;
        private bool _hasAttacked;
        
        public AttackState(CopyCatController controller) : base(controller)
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

            // 랜덤하게 다음 상태 결정
            if (TriggerCalled)
            {
                if (Random.Range(0f, 1f) < 0.5f)
                    Controller.ChangeMoveState();
                else
                    Controller.ChangeCastingState();
            }
        }
    }
    
    // Casting State
    public class CastingState : CopyCatStateBase
    {
        public CastingState(CopyCatController controller) : base(controller)
        {
        }
        
        protected override string GetAnimationName() => "Casting";
        
        public override void Execute()
        {
            base.Execute();
            
            if (Controller.IsOnMove) 
                Controller.ChangeMoveState();
                
            if (TriggerCalled)
            {
                Controller.ChangeAttackState();
            }
        }
    }

    // Move State
    public class MoveState : CopyCatStateBase
    {
        public MoveState(CopyCatController controller) : base(controller)
        {
        }

        protected override string GetAnimationName() => "Move";
   
        public override void Execute()
        {
            base.Execute();
            Controller.ExecuteMove();
            // 움직임이 멈추면 Casting 상태로 전환
            if (Controller.IsOnMove is false)
                Controller.ChangeCastingState();
        }
    }

    // Skill State
    public class SkillState : CopyCatStateBase
    {
        private readonly SkillBase _skill;
        
        public SkillState(CopyCatController controller, SkillBase skill) : base(controller)
        {
            _skill = skill;
        }

        protected override string GetAnimationName() => _skill.AnimationName;

        public override void Enter()
        {
            base.Enter();
            Controller.FaceTarget();
            _skill.ExecuteSkill().Forget();
        }

        public override void Execute()
        {
            Debug.Log($"SkillState TriggerCalled: {TriggerCalled}");
            base.Execute();
            
            if(TriggerCalled)
                Controller.ChangeMoveState();
        }

        public override void Exit()
        {
            base.Exit();
        }
    }

    // FadeIn State - SpriteRenderer 기반 페이드인 효과 (UniTask 사용)
    public class FadeInState : CopyCatStateBase
    {
        private float fadeTime = 0.5f;
        private SpriteRenderer _spriteRenderer;
        private CancellationTokenSource _fadeCancellationToken;
        
        public bool isDone { get; private set; }

        public FadeInState(CopyCatController controller) : base(controller)
        {
            _spriteRenderer = Controller.GetComponent<SpriteRenderer>();
            if (_spriteRenderer == null)
            {
                Debug.LogWarning("SpriteRenderer not found on CopyCat GameObject!");
            }
        }

        protected override string GetAnimationName() => "Idle";

        public override void Enter()
        {
            base.Enter();
            isDone = false;
            
            if (_spriteRenderer != null)
            {
                var color = _spriteRenderer.color;
                color.a = 0f;
                _spriteRenderer.color = color;
                
                _fadeCancellationToken = new CancellationTokenSource();
                FadeInAsync(_fadeCancellationToken.Token).Forget();
            }
            else
            {
                isDone = true;
            }
        }

        private async UniTask FadeInAsync(CancellationToken cancellationToken)
        {
            try
            {
                var time = 0f;

                while (time < fadeTime)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    
                    time += Time.deltaTime;
                    var spriteRendererColor = _spriteRenderer.color;
                    spriteRendererColor.a = Mathf.Lerp(0f, 1f, time / fadeTime);
                    _spriteRenderer.color = spriteRendererColor;
                    
                    await UniTask.Yield(cancellationToken);
                }
                
                // 완전히 불투명하게 설정
                var finalColor = _spriteRenderer.color;
                finalColor.a = 1f;
                _spriteRenderer.color = finalColor;
                
                isDone = true;
            }
            catch (System.OperationCanceledException)
            {
                Debug.Log("FadeIn cancelled");
            }
        }

        public override void Execute()
        {
            base.Execute();
            // 페이드인 완료 시 Casting 상태로 전환
            if (isDone)
            {
                Controller.ChangeCastingState();
            }
        }
        
        public override void Exit()
        {
            base.Exit();
            isDone = false;
            
            if (_fadeCancellationToken != null)
            {
                _fadeCancellationToken.Cancel();
                _fadeCancellationToken.Dispose();
                _fadeCancellationToken = null;
            }
        }
    }

    // FadeOut State - SpriteRenderer 기반 페이드아웃 효과 (UniTask 사용)
    public class FadeOutState : CopyCatStateBase
    {
        private float fadeTime = 0.5f;
        private SpriteRenderer _spriteRenderer;
        private CancellationTokenSource _fadeCancellationToken;
        
        public bool IsDone { get; private set; }

        public FadeOutState(CopyCatController controller) : base(controller)
        {
            _spriteRenderer = Controller.GetComponent<SpriteRenderer>();
            if (_spriteRenderer == null)
            {
                Debug.LogWarning("SpriteRenderer not found on CopyCat GameObject!");
            }
        }

        protected override string GetAnimationName() => "Idle";

        public override void Enter()
        {
            base.Enter();
            IsDone = false;
            
            if (_spriteRenderer != null)
            {
                _fadeCancellationToken = new CancellationTokenSource();
                FadeOutAsync(_fadeCancellationToken.Token).Forget();
            }
            else
            {
                IsDone = true;
            }
        }

        private async UniTask FadeOutAsync(CancellationToken cancellationToken)
        {
            try
            {
                var time = 0f;

                while (time < fadeTime)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    
                    time += Time.deltaTime;
                    var spriteRendererColor = _spriteRenderer.color;
                    spriteRendererColor.a = Mathf.Lerp(1f, 0f, time / fadeTime);
                    _spriteRenderer.color = spriteRendererColor;
                    
                    await UniTask.Yield(cancellationToken);
                }

                // 완전히 투명하게 설정
                var finalColor = _spriteRenderer.color;
                finalColor.a = 0f;
                _spriteRenderer.color = finalColor;
                
                IsDone = true;
            }
            catch (System.OperationCanceledException)
            {
                Debug.Log("FadeOut cancelled");
            }
        }

        public override void Execute()
        {
            base.Execute();
            
            // 페이드아웃 완료 시 GameObject 비활성화
            if (IsDone)
            {
                Controller.gameObject.SetActive(false);
            }
        }
        
        public override void Exit()
        {
            base.Exit();
            
            if (_fadeCancellationToken != null)
            {
                _fadeCancellationToken.Cancel();
                _fadeCancellationToken.Dispose();
                _fadeCancellationToken = null;
            }
        }
    }

    // Dead State
    public class DeadState : CopyCatStateBase
    {
        public DeadState(CopyCatController controller) : base(controller)
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
    public class IdleState : CopyCatStateBase
    {

        public IdleState(CopyCatController controller) : base(controller)
        {
        }
        public override void Enter()
        {
            base.Enter();
            if (Controller.Rigidbody2D != null)
                Controller.Rigidbody2D.simulated = false;
        }
        protected override string GetAnimationName() => "Idle";

    }
}