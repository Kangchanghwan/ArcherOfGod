using System;
using System.Collections.Generic;
using Component.Attack;
using Component.Input;
using Component.Skill;
using Component.SkillSystem;
using Controller.Entity;
using Interface;
using Model;
using UI;
using UnityEngine;
using Util;
using static Model.EnemyModel;
using StateMachine = Component.StateSystem.StateMachine;

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
            _model = new EnemyModel(
                maxHealth: maxHealth
            );

            StateMachine = new StateMachine();
            // Enemy 전용 State로 교체
            _attackState = new AttackState(this);
            _castingState = new CastingState(this);
            _moveState = new MoveState(this);
            _idleState = new IdleState(this);
            _deadState = new DeadState(this);

            shotArrow = GetComponent<ShotArrow>();
            uiHealthBar = GetComponent<UI_HealthBar>();
            aiInputSystem = GetComponent<AIInputSystem>();
            
            Debug.Assert(shotArrow != null);
            Debug.Assert(uiHealthBar != null);
            Debug.Assert(aiInputSystem != null, "AIInputSystem component is required!");
            
            var skillBases = GetComponentsInChildren<SkillBase>(true);
            foreach (var skill in skillBases)
            {
                skill.Initialize(rigidbody: Rigidbody2D, anim: Animator);
                _skills.Add(skill.SkillType, skill); 
            }
            
            _model.OnDeath += HandleOnDeath;
            
            EventManager.Publish(new OnEntitySpawnEvent(EntityType.Enemy, this));
            EventManager.Subscribe<OnGameStartEvent>(HandleOnGameStart);
            
            StateMachine.Initialize(_idleState);
        }

        public override void AnimationTrigger()
        {
            if (StateMachine.CurrentState is EntityStateBase<EnemyController> currentState)
            {
                currentState.AnimationTrigger(); 
            }
        }
        
        public void HandleInputSystem(bool onOff)
        {
            // AI는 InputManager를 사용하지 않으므로 빈 구현
        }
        private void Update()
        {
            StateMachine.CurrentState.Execute();
        }


        private void OnDisable()
        {
            _model.OnDeath -= HandleOnDeath;
            EventManager.Unsubscribe<OnGameStartEvent>(HandleOnGameStart);
        }
        
        private void HandleOnGameStart(OnGameStartEvent @event) => ChangeCastingState();

        // AI 입력 시스템 기반 이동
        public void ExecuteMove()
        {
            float xInput = aiInputSystem != null ? aiInputSystem.HorizontalInput : 0f;
            FlipController(xInput);
            var movement = new Vector2(xInput * moveSpeed * Time.deltaTime, Rigidbody2D.linearVelocity.y);
            Rigidbody2D.MovePosition(Rigidbody2D.position + movement);
        }

        public void SetTarget(Transform transform) => Target = transform;

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
            StateMachine.ChangeState(_deadState);
            Rigidbody2D.simulated = false;
            EventManager.Publish(new OnEntityDeathEvent(EntityType.Enemy));
        }


        public void ChangeMoveState() => StateMachine.ChangeState(_moveState);
        public void ChangeAttackState() => StateMachine.ChangeState(_attackState);
        public void ChangeCastingState() => StateMachine.ChangeState(_castingState);
    }
}