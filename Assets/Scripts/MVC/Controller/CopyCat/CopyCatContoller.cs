using System.Collections.Generic;
using Component.Attack;
using Component.Input;
using Component.Skill;
using Controller.Entity;
using Model;
using UI;
using UnityEngine;
using Util;
using StateMachine = Component.StateSystem.StateMachine;

namespace MVC.Controller.CopyCat
{
    public class CopyCatController : EntityControllerBase
    {

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
        [SerializeField]
        private int healthDrainPerSecond = 10;

        [Header("Component")]
        [SerializeField] private UI_HealthBar uiHealthBar;
        [SerializeField] private ShotArrow shotArrow;

        // AI 시스템 추가
        [Header("AI System")] [SerializeField] private AIInputSystem aiInputSystem;

        #region state

        private AttackState _attackState;
        private CastingState _castingState;
        private MoveState _moveState;
        private FadeInState _fadeInState;
        private FadeOutState _fadeOutState;

        #endregion

        private CopyCatModel _model;
        private readonly Dictionary<SkillType, SkillBase> _skills = new();

        // 체력 감소 타이머
        private float _healthDrainTimer;

        // AI 입력 시스템 기반으로 변경
        public bool IsOnMove => Mathf.Abs(aiInputSystem.HorizontalInput) > 0.1f;
        public float AttackDelay => attackDelay;

        protected override void Awake()
        {
            base.Awake();
            _model = new CopyCatModel(
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

            shotArrow = GetComponent<ShotArrow>();
            uiHealthBar = GetComponent<UI_HealthBar>();
            aiInputSystem = GetComponent<AIInputSystem>();
            

            var skillBases = GetComponentsInChildren<SkillBase>(true);
            foreach (var skill in skillBases)
            {
                skill.Initialize(rigidbody: Rigidbody2D, anim: Animator);
                _skills.Add(skill.SkillType, skill);
            }
            

            Debug.Assert(shotArrow != null);
            Debug.Assert(uiHealthBar != null);
            Debug.Assert(aiInputSystem != null, "AIInputSystem component is required!");
        }

        public override void AnimationTrigger()
        {
            if (StateMachine.CurrentState is EntityStateBase<CopyCatController> currentState)
            {
                currentState.AnimationTrigger(); 
            }
        }

        private void OnEnable()
        {
            _model.OnDeath += HandleOnDeath;
            _model.OnHealthUpdate += HandleHealthUpdate;
            EventManager.Publish(new OnEntitySpawnEvent(EntityType.CopyCat, this));
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
                _healthDrainTimer = 0f;
            }
        }

        private void OnDisable()
        {
            _model.OnDeath -= HandleOnDeath;
            _model.OnHealthUpdate -= HandleHealthUpdate;
        }

        // AI 입력 시스템 기반 이동
        public void ExecuteMove()
        {
            float xInput = aiInputSystem != null ? aiInputSystem.HorizontalInput : 0f;
            FlipController(xInput);
            var movement = new Vector2(xInput * moveSpeed * Time.deltaTime, Rigidbody2D.linearVelocity.y);
            Rigidbody2D.MovePosition(Rigidbody2D.position + movement);
        }

        public override void TakeDamage(float damage)
        {
            _model.TakeDamage((int)damage);
            uiHealthBar.UpdateHealthBar(_model.GetCurrentHealth(), _model.GetMaxHealth());
        }

        public override void TargetOnDead()
        {
            StateMachine.ChangeState(_fadeOutState);
            Rigidbody2D.simulated = false;
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

        private void HandleOnDeath()
        {
            StateMachine.ChangeState(_fadeOutState);
            Rigidbody2D.simulated = false;
            EventManager.Publish(new OnEntityDeathEvent(EntityType.CopyCat));
        }

        private void HandleHealthUpdate() => EventManager.Publish(
            new OnHealthUpdateEvent(
                type: EntityType.CopyCat,
                maxHealth: _model.GetMaxHealth(),
                currentHealth: _model.GetCurrentHealth())
        );


        public void ChangeMoveState() => StateMachine.ChangeState(_moveState);
        public void ChangeAttackState() => StateMachine.ChangeState(_attackState);
        public void ChangeCastingState() => StateMachine.ChangeState(_castingState);
    }
}