using System.Collections.Generic;
using Component.Attack;
using Component.Input;
using Component.Skill;
using MVC.Data;
using UI;
using UnityEngine;
using Util;

namespace MVC.Controller.Enemy
{
    public class EnemyController : EntityControllerBase
    {
        [Header("Enemy Model Info")] [SerializeField]
        private int attack;

        [SerializeField] private int maxHealth;

        [Header("Enemy Attack Info")] [SerializeField]
        private float attackSpeed = 1f;

        [SerializeField] private Vector2 firePointOffset;
        [SerializeField] private float arrowSpeed;
        [SerializeField] private float attackDelay = 0.62f;

        [Space] [Header("Enemy Move Info")] [SerializeField]
        private float moveSpeed = 10f;

        [Header("Component")] 
        [SerializeField] private UI_HealthBar uiHealthBar;
        [SerializeField] private ShotArrow shotArrow;

        // AI 시스템 추가
        [Header("AI System")]
        [SerializeField] private AIInputSystem aiInputSystem;

        #region state
        // Enemy 전용 State 클래스들로 변경
        private AttackState _attackState;
        private CastingState _castingState;
        private MoveState _moveState;
        private IdleState _idleState;
        private DeadState _deadState;
        #endregion

        private EnemyModel _model;
        private Dictionary<SkillType, SkillBase> _skills = new();

        // AI 입력 시스템 기반으로 변경
        public bool IsOnMove => Mathf.Abs(aiInputSystem.HorizontalInput) > 0.1f;
        public float AttackDelay => attackDelay;

        public override void Init()
        {
            base.Init();
            InitializeModel();
            InitializeStateMachine();
            InitializeStates();
            InitializeComponents();
            InitializeSkills();
            RegisterEventHandlers();
            StartStateMachine();
            
            GetComponent<AnimationEvent>().Init();
        }

        private void InitializeModel()
        {
            _model = new EnemyModel(
                maxHealth: maxHealth
            );
        }

        private void InitializeStateMachine()
        {
            StateMachine = new StateMachine();
        }

        private void InitializeStates()
        {
            _attackState = new AttackState(this);
            _castingState = new CastingState(this);
            _moveState = new MoveState(this);
            _idleState = new IdleState(this);
            _deadState = new DeadState(this);
        }

        private void InitializeComponents()
        {
            shotArrow = GetComponent<ShotArrow>();
            uiHealthBar = GetComponent<UI_HealthBar>();
            aiInputSystem = GetComponent<AIInputSystem>();

            Debug.Assert(shotArrow != null);
            Debug.Assert(uiHealthBar != null);
            Debug.Assert(aiInputSystem != null, "AIInputSystem component is required!");
        }

        private void InitializeSkills()
        {
            var skillBases = GetComponentsInChildren<SkillBase>(true);
            foreach (var skill in skillBases)
            {
                skill.Initialize(rigidbody: Rigidbody2D, anim: Animator);
                _skills.Add(skill.SkillType, skill);
            }
        }

        private void RegisterEventHandlers()
        {
            _model.OnDeath += HandleOnDeath;
        }

        private void StartStateMachine()
        {
            StateMachine.Initialize(_idleState);
        }

        public override EntityType GetEntityType() => EntityType.Enemy;

        public void HandleInputSystem(bool onOff)
        {
            // AI는 InputManager를 사용하지 않으므로 빈 구현
        }
        private void Update()
        {
            if (StateMachine?.CurrentState == null)
                return;
    
            // 구체 타입으로 캐스팅 시도
            if (StateMachine.CurrentState is EntityStateBase state)
                state.Execute();
            else
                StateMachine.CurrentState.Execute();
        }


        private void OnDisable()
        {
            _model.OnDeath -= HandleOnDeath;
        }

        // AI 입력 시스템 기반 이동
        public void ProcessMovement()
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
            StateMachine.ChangeState(_idleState);
            Rigidbody2D.simulated = false;
        }

        public void PrepareAttack()
        {
            Animator.SetFloat("AttackSpeed", attackSpeed);
            FaceTarget();
        }

        public void PerformAttack()
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
            StateMachine.ChangeState(_deadState);
            Rigidbody2D.simulated = false;
            EventManager.Publish(new OnEntityDeathEvent(EntityType.Enemy));
        }
        
        public void ChangeMoveState() => StateMachine.ChangeState(_moveState);
        public void ChangeAttackState() => StateMachine.ChangeState(_attackState);
        public void ChangeCastingState() => StateMachine.ChangeState(_castingState);
    }
}