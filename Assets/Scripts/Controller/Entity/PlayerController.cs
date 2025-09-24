using System.AttackSystem;
using System.Collections;
using System.Threading;
using Cysharp.Threading.Tasks;
using Model;
using UnityEngine;
using StateMachine = System.StateSystem.StateMachine;

namespace Controller.Entity
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
        [Space]
        [Header("Player Move Info")]
        [SerializeField]private float moveSpeed = 10f;

        [Header("Component")] [SerializeField] private HealthSystem healthSystem;
        [SerializeField] private ShotArrow shotArrow;

        
        #region state

        private Component.Entity.AttackState _attackState;
        private Component.Entity.CastingState _castingState;
        private Component.Entity.MoveState _moveState;

        #endregion

        public StateMachine StateMachine { get; private set; }
        private PlayerModel _model;
        private InputManager _inputManager;

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

            healthSystem = GetComponent<HealthSystem>();
            shotArrow = GetComponent<ShotArrow>();

            Debug.Assert(healthSystem != null);
            Debug.Assert(shotArrow != null);
        }


        private void OnEnable()
        {
            _inputManager.Enable();
            _inputManager.Controller.Move.performed += ctx => _xInput = ctx.ReadValue<float>();
            _inputManager.Controller.Move.canceled += _ => _xInput = 0f;

            _model.OnPlayerDeath += HandlePlayerDeath;
        }

        private void Start()
        {
            StateMachine.Initialize(_castingState);
        }

        private void Update()
        {
            StateMachine.CurrentState.Execute();
        }

        private void OnDisable()
        {
            _inputManager.Disable();
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

        private void HandlePlayerDeath()
        {
            //사망시 이벤트 작성
        }

        // protected override Transform SetTarget() => GameManager.Instance.PlayerOfTarget.GetTransform();
        public Transform GetTransform() => transform;

        public void ChangeMoveState() => StateMachine.ChangeState(_moveState);
        public void ChangeAttackState() => StateMachine.ChangeState(_attackState);
        public void ChangeCastingState() => StateMachine.ChangeState(_castingState);
    }
}