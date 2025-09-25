using System;
using System.AttackSystem;
using System.Collections.Generic;
using System.InputSystem;
using System.SkillSystem;
using Model;
using UnityEngine;
using StateMachine = Component.StateSystem.StateMachine;

namespace Controller.Entity.CopyCat
{
    public class CopyCatController : EntityBase, IDamageable
    {
        public static event Action OnCopyCatDeath;
        
        [Header("CopyCat Model Info")] [SerializeField]
        private int attack;

        [SerializeField] private int maxHealth;

        [Header("CopyCat Attack Info")] [SerializeField]
        private float attackSpeed = 1f;

        [SerializeField] private Vector2 firePointOffset;
        [SerializeField] private float arrowSpeed;
        [SerializeField] private float attackDelay = 0.62f;

        [Space] [Header("CopyCat Move Info")] [SerializeField]
        private float moveSpeed = 10f;

        [Header("CopyCat Unique Settings")] [SerializeField]
        private int healthDrainPerSecond = 10;

        [Header("Component")] 
        [SerializeField] private HealthSystem healthSystem;
        [SerializeField] private ShotArrow shotArrow;

        // AI 시스템 추가
        [Header("AI System")]
        [SerializeField] private AIInputSystem aiInputSystem;

        #region state
        // CopyCat도 동일한 State 사용 (또는 CopyCat 전용 State 생성 가능)
        private AttackState _attackState;
        private CastingState _castingState;
        private MoveState _moveState;
        private FadeInState _fadeInState;
        private FadeOutState _fadeOutState;
        #endregion

        private CopyCatModel _model;
        private Dictionary<SkillType, SkillBaseV2> _skills = new();

        // 체력 감소 타이머
        private float _healthDrainTimer;

        // AI 입력 시스템 기반으로 변경
        public bool IsOnMove => Mathf.Abs(aiInputSystem.HorizontalInput) > 0.1f;
        public float AttackDelay => attackDelay;

        protected override void Awake()
        {
            base.Awake();
            _model = new CopyCatModel(
                attack: attack,
                maxHealth: maxHealth,
                healthDrainPerSecond: healthDrainPerSecond
            );

            StateMachine = new StateMachine();
            // State 클래스들 초기화
            _attackState = new AttackState(this);
            _castingState = new CastingState(this);
            _moveState = new MoveState(this);
            _fadeInState = new FadeInState(this);
            _fadeOutState = new FadeOutState(this);
            
            healthSystem = GetComponent<HealthSystem>();
            shotArrow = GetComponent<ShotArrow>();
            
            aiInputSystem = GetComponent<AIInputSystem>();

            var skillBases = GetComponentsInChildren<SkillBaseV2>(true);
            foreach (var skill in skillBases)
            {
                skill.Initialize(
                    rigidbody: Rigidbody2D,
                    anim: Animator,
                    target: Target
                );
                _skills.Add(skill.SkillType, skill);
            }

            Debug.Assert(healthSystem != null);
            Debug.Assert(shotArrow != null);
            Debug.Assert(aiInputSystem != null, "AIInputSystem component is required!");
        }

        private void OnEnable()
        {
            _model.OnCopyCatDeath += HandleCopyCatDeath;
        }

        public void HandleInputSystem(bool onOff)
        {
            // AI는 InputManager를 사용하지 않으므로 빈 구현
        }

        private void Start()
        {
            StateMachine.Initialize(_fadeInState);
            _healthDrainTimer = 0f;
        }

        private void Update()
        {
            StateMachine.CurrentState.Execute();
            HandleHealthDrain();
        }

        // CopyCat 고유 기능: 시간이 지나면서 체력 감소
        private void HandleHealthDrain()
        {
            _healthDrainTimer += Time.deltaTime;

            if (_healthDrainTimer >= 1f)
            {
                _model.DrainHealth();
                healthSystem.UpdateHealthBar(
                    currentHealth: _model.GetCurrentHealth(),
                    maxHealth: _model.GetMaxHealth()
                );
                _healthDrainTimer = 0f;
            }
        }

        private void OnDisable()
        {
            _model.OnCopyCatDeath -= HandleCopyCatDeath;
        }

        // AI 입력 시스템 기반 이동
        public void ExecuteMove()
        {
            float xInput = aiInputSystem != null ? aiInputSystem.HorizontalInput : 0f;
            FlipController(xInput);
            var movement = new Vector2(xInput * moveSpeed * Time.deltaTime, Rigidbody2D.linearVelocity.y);
            Rigidbody2D.MovePosition(Rigidbody2D.position + movement);
        }

        public void TakeDamage(float damage)
        {
            _model.TakeDamage((int)damage);
            healthSystem.UpdateHealthBar(
                currentHealth: _model.GetCurrentHealth(),
                maxHealth: _model.GetMaxHealth()
            );
        }

        public void AttackReady()
        {
            Animator.SetFloat("AttackSpeed", attackSpeed);
            FaceTarget();
        }

        public void ExecuteAttack()
        {
            // 화살 공격 실행
            shotArrow.Attack(new ShotArrowCommand(
                damage: attack,
                duration: arrowSpeed,
                startPoint: (Vector2)transform.position + firePointOffset,
                endPoint: Target
            ));
        }

        private void HandleCopyCatDeath()
        {
            Animator.SetTrigger("Dead");
            Rigidbody2D.simulated = false;
            OnCopyCatDeath?.Invoke();
        }

        public Transform GetTransform() => transform;

        public void ChangeMoveState() => StateMachine.ChangeState(_moveState);
        public void ChangeAttackState() => StateMachine.ChangeState(_attackState);
        public void ChangeCastingState() => StateMachine.ChangeState(_castingState);
    }
}