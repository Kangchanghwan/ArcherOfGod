using System;
using System.Collections.Generic;
using Component.Attack;
using Component.Skill;
using Component.SkillSystem;
using Controller.Entity;
using Interface;
using Model;
using MVC.Data;
using UI;
using UnityEngine;
using Util;
using StateMachine = Component.StateSystem.StateMachine;

namespace MVC.Controller.Player
{
    public class PlayerController : EntityControllerBase
    {
        [Header("Player Model Info")] [SerializeField]
        private int attack;

        [SerializeField] private int maxHealth;

        [Header("Player Attack Info")] [SerializeField]
        private float attackSpeed = 1f;

        [SerializeField] private Vector2 firePointOffset;
        [SerializeField] private float arrowSpeed;
        [SerializeField] private float attackDelay = 0.62f;

        [Space] [Header("Player Move Info")] [SerializeField]
        private float moveSpeed = 10f;

        [Header("Component")] [SerializeField] private UI_HealthBar uiHealthBar;
        [SerializeField] private ShotArrow shotArrow;

        #region state

        private AttackState _attackState;
        private CastingState _castingState;
        private MoveState _moveState;
        private IdleState _idleState;
        private DeadState _deadState;

        #endregion

        private PlayerModel _model;
        private InputManager _inputManager;
        private readonly Dictionary<SkillType, SkillBase> _skills = new();
        private float _xInput;

        public bool IsOnMove => Mathf.Abs(_xInput) > 0.1f;
        public float AttackDelay => attackDelay;

        protected override void Awake()
        {
            base.Awake();
            _model = new PlayerModel(
                maxHealth: maxHealth
            );

            _inputManager = new InputManager();
            StateMachine = new StateMachine();

            _attackState = new(this);
            _castingState = new(this);
            _moveState = new(this);
            _idleState = new(this);
            _deadState = new(this);

            shotArrow = GetComponent<ShotArrow>();
            uiHealthBar = GetComponent<UI_HealthBar>();


            Debug.Assert(uiHealthBar != null);
            Debug.Assert(shotArrow != null);
        }

        public override void AnimationTrigger()
        {
            if (StateMachine.CurrentState is EntityStateBase<PlayerController> currentState)
            {
                currentState.AnimationTrigger();
            }
        }


        private void OnEnable()
        {
            HandleInputSystem(true);
            
            
            _inputManager.Controller.Move.performed += ctx => _xInput = ctx.ReadValue<float>();
            _inputManager.Controller.Move.canceled += _ => _xInput = 0f;

            _inputManager.Controller.Skill_1.performed += _ => TryUseSkill(SkillType.JumpShoot);
            _inputManager.Controller.Skill_2.performed += _ => TryUseSkill(SkillType.BombShoot);
            _inputManager.Controller.Skill_3.performed += _ => TryUseSkill(SkillType.RepidFire);
            _inputManager.Controller.Skill_4.performed += _ => TryUseSkill(SkillType.WhirlWind);
            _inputManager.Controller.Skill_5.performed += _ => TryUseSkill(SkillType.CopyCat);

            _model.OnDeath += HandleOnDeath;
            _model.OnHealthUpdate += HandleHealthUpdate;
            
            EventManager.Publish(new OnEntitySpawnEvent(EntityType.Player, this));
            EventManager.Subscribe<OnGameStartEvent>(HandleOnGameStart);
        }

        public void HandleInputSystem(bool onOff)
        {
            if (onOff)
                _inputManager.Enable();
            else
                _inputManager.Disable();
        }

        private void Start()
        {
            StateMachine.Initialize(_idleState);
        }

        private void Update()
        {
            StateMachine.CurrentState.Execute();
        }

        private void HandleOnGameStart(OnGameStartEvent @event)
        {
            ChangeCastingState();
            
            var skillBases = GetComponentsInChildren<SkillBase>(true);
            foreach (var skill in skillBases)
            {
                skill.Initialize(rigidbody: Rigidbody2D, anim: Animator);
                _skills.Add(skill.SkillType, skill);
            }
        } 
        private void TryUseSkill(SkillType skillType)
        {
            SkillBase skill = _skills[skillType];

            if (skill == null) return;
            if (skill.CanUseSkill() == false) return;

            skill.SetTarget(Target);
            StateMachine.ChangeState(new SkillState(this, skill));
        }

        private void OnDisable()
        {
            HandleInputSystem(false);
            _model.OnDeath -= HandleOnDeath;
            _model.OnHealthUpdate -= HandleHealthUpdate;
            EventManager.Unsubscribe<OnGameStartEvent>(HandleOnGameStart);
        }

        public void ExecuteMove()
        {
            FlipController(_xInput);
            var movement = new Vector2(_xInput * moveSpeed * Time.deltaTime, Rigidbody2D.linearVelocity.y);
            Rigidbody2D.MovePosition(Rigidbody2D.position + movement);
        }


        public void SetTarget(Transform transform) => Target = transform;

        public override void TakeDamage(float damage)
        {
            _model.UpdateCurrentHealth((int)damage);
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
            EventManager.Publish(new OnEntityDeathEvent(EntityType.Player));
        }

        private void HandleHealthUpdate() => EventManager.Publish(
            new OnHealthUpdateEvent(
                type: EntityType.Player,
                maxHealth: _model.GetMaxHealth(),
                currentHealth: _model.GetCurrentHealth())
        );

        public void ChangeMoveState() => StateMachine.ChangeState(_moveState);
        public void ChangeAttackState() => StateMachine.ChangeState(_attackState);
        public void ChangeCastingState() => StateMachine.ChangeState(_castingState);
        public void ChangeIdleState() => StateMachine.ChangeState(_idleState);
    }
}