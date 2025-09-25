using System.Collections.Generic;
using Component.Attack;
using Component.Skill;
using Component.SkillSystem;
using Controller.Entity;
using Model;
using UI;
using UnityEngine;
using StateMachine = Component.StateSystem.StateMachine;

namespace MVC.Controller.Player
{
    public class PlayerController : EntityBase, IDamageable
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

        #endregion

        private PlayerModel _model;
        private InputManager _inputManager;
        private Dictionary<SkillType, SkillBase> _skills = new();
        private float _xInput;
        
        public bool IsOnMove => Mathf.Abs(_xInput) > 0.1f;
        public float AttackDelay => attackDelay;

        protected override void Awake()
        {
            base.Awake();
            _model = new PlayerModel(
                attack: attack,
                maxHealth: maxHealth
            );
            _inputManager = new InputManager();

            StateMachine = new StateMachine();
            _attackState = new(this);
            _castingState = new(this);
            _moveState = new(this);

            uiHealthBar = GetComponent<UI_HealthBar>();
            shotArrow = GetComponent<ShotArrow>();

            var skillBases = GetComponentsInChildren<SkillBase>(true);
            foreach (var skill in skillBases)
            {
                skill.Initialize(
                    rigidbody: Rigidbody2D,
                    anim: Animator,
                    target: Target
                );
                _skills.Add(skill.SkillType, skill);
            }


            Debug.Assert(uiHealthBar != null);
            Debug.Assert(shotArrow != null);
        }


        private void OnEnable()
        {
            HandleInputSystem(true);
            _inputManager.Controller.Move.performed += ctx => _xInput = ctx.ReadValue<float>();
            _inputManager.Controller.Move.canceled += _ => _xInput = 0f;

            _model.OnPlayerDeath += HandlePlayerDeath;
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
            StateMachine.Initialize(_castingState);
        }

        private void Update()
        {
            StateMachine.CurrentState.Execute();
            HandleOnSkillInput();
        }

        private void HandleOnSkillInput()
        {
            SkillBase skill = null;

            if (_inputManager.Controller.Skill_1.WasPerformedThisFrame())
                skill = _skills[SkillType.JumpShoot];
            if (_inputManager.Controller.Skill_2.WasPerformedThisFrame())
                skill = _skills[SkillType.BombShoot];
            if (_inputManager.Controller.Skill_3.WasPerformedThisFrame())
                skill = _skills[SkillType.RepidFire];
            if (_inputManager.Controller.Skill_4.WasPerformedThisFrame())
                skill = _skills[SkillType.WhirlWind];
            if (_inputManager.Controller.Skill_5.WasPerformedThisFrame())
                skill = _skills[SkillType.CopyCat];
            
            if (skill == null) return;
            if (skill.CanUseSkill() == false) return;
            
            StateMachine.ChangeState(new SkillState(this, skill));
        }

        private void OnDisable()
        {
            HandleInputSystem(false);
            _model.OnPlayerDeath -= HandlePlayerDeath;
        }

        public void ExecuteMove()
        {
            FlipController(_xInput);
            var movement = new Vector2(_xInput * moveSpeed * Time.deltaTime, Rigidbody2D.linearVelocity.y);
            Rigidbody2D.MovePosition(Rigidbody2D.position + movement);
        }

        public void TakeDamage(float damage)
        {
            _model.TakeDamage((int)damage);
            uiHealthBar.UpdateHealthBar(
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

        private void HandlePlayerDeath()
        {
            Animator.SetTrigger("Dead");
            Rigidbody2D.simulated = false;
        }

        private void HandleEnemyDeath()
        {
            Animator.SetTrigger("Idle");
            Rigidbody2D.simulated = false;
        }

        // protected override Transform SetTarget() => GameManager.Instance.PlayerOfTarget.GetTransform();
        public Transform GetTransform() => transform;

        public void ChangeMoveState() => StateMachine.ChangeState(_moveState);
        public void ChangeAttackState() => StateMachine.ChangeState(_attackState);
        public void ChangeCastingState() => StateMachine.ChangeState(_castingState);
    }
}