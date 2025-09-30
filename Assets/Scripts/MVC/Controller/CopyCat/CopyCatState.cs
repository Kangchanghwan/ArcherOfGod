using System.Threading;
using Component.Skill;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Scripting;

namespace MVC.Controller.CopyCat
{
    public class AttackState : EntityStateBase
    {
        private float _attackTimer;
        private bool _hasAttacked;
        private readonly CopyCatController _controller;
        
        public AttackState(CopyCatController controller) : base(controller)
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

            // 랜덤하게 다음 상태 결정
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
        private readonly CopyCatController _controller;
        public CastingState(CopyCatController controller) : base(controller)
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
            {
                _controller.ChangeAttackState();
            }
        }
    }
    public class MoveState : EntityStateBase
    {
        private readonly CopyCatController _controller;

        public MoveState(CopyCatController controller) : base(controller)
        {
            _controller = controller;
        }

        protected override string GetAnimationName() => "Move";
   
        public override void Execute()
        {
            base.Execute();
            _controller.ProcessMovement();
            // 움직임이 멈추면 Casting 상태로 전환
            if (_controller.IsOnMove is false)
                _controller.ChangeCastingState();
        }
    }
    public class SkillState : EntityStateBase
    {
        private readonly SkillBase _skill;
        private readonly CopyCatController _controller;
        
        public SkillState(CopyCatController controller, SkillBase skill) : base(controller)
        {
            _skill = skill;
            _controller = controller;
        }

        protected override string GetAnimationName() => _skill.AnimationName;

        public override void Enter()
        {
            base.Enter();
            _controller.FaceTarget();
            _skill.ExecuteSkill().Forget();
        }

        public override void Execute()
        {
            Debug.Log($"SkillState TriggerCalled: {TriggerCalled}");
            base.Execute();
            
            if(TriggerCalled)
                _controller.ChangeMoveState();
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
    // FadeIn State - SpriteRenderer 기반 페이드인 효과 (UniTask 사용)
    public class FadeInState : EntityStateBase
    {
        private float fadeTime = 0.5f;
        private SpriteRenderer _spriteRenderer;
        private CancellationTokenSource _fadeCancellationToken;
        private readonly CopyCatController _controller;

        
        public bool isDone { get; private set; }

        public FadeInState(CopyCatController controller) : base(controller)
        {
            _spriteRenderer = Controller.GetComponent<SpriteRenderer>();
            if (_spriteRenderer == null)
            {
                Debug.LogWarning("SpriteRenderer not found on CopyCat GameObject!");
            }
            _controller = controller;
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
                _controller.ChangeCastingState();
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
    public class FadeOutState : EntityStateBase
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
}