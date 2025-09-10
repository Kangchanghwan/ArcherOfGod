using System.Collections;
using UnityEngine;

public class EnemyAttackState : EnemyState
{
    private AttackBase _attackBase;
    [SerializeField] private float attackSpeed;
    [SerializeField] private Vector2 firePointOffset;
    [SerializeField] private int damage;
    [SerializeField] private float arrowSpeed;
    [SerializeField] private Coroutine _currentCoroutine;

    protected override string GetAnimationName() => "Attack";

    private void Start()
    {
        _attackBase = GetComponent<AttackBase>();
    }

    public override void Enter()
    {
        base.Enter();
        Animator.SetFloat("AttackSpeed", attackSpeed);
        Enemy.FlipController();
        _currentCoroutine = StartCoroutine(Attack());
    }

    private IEnumerator Attack()
    {
        yield return new WaitForSeconds(0.62f/attackSpeed);

        _attackBase.Attack(
            (Vector2)transform.position + firePointOffset,
            GameManager.Instance.EnemyOfTarget.GetTransform()
        );
        yield return new WaitUntil(() => TriggerCalled);
    }

    public override void Exit()
    {
        base.Exit();
        StopCoroutine(_currentCoroutine);
    }
}