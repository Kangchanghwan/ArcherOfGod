using System.Collections;
using UnityEngine;

public class AttackState : StateBase
{
    private AttackBase _attackBase;
    private Coroutine _currentCoroutine;
    
    [SerializeField] private float attackSpeed = 1f;
    [SerializeField] private Vector2 firePointOffset;
    [SerializeField] private int damage;
    [SerializeField] private float arrowSpeed;
    [SerializeField] private float attackDelay = 0.62f;

    protected override string GetAnimationName() => "Attack";

    protected override void Start()
    {
        base.Start();
        _attackBase = GetComponent<AttackBase>();
    }

    public override void Enter()
    {
        base.Enter();
        Animator.SetFloat("AttackSpeed", attackSpeed);
        Entity.FaceTarget();
        _currentCoroutine = StartCoroutine(Attack());
    }

    private IEnumerator Attack()
    {
        yield return new WaitForSeconds(attackDelay / attackSpeed);

        _attackBase.Attack(
            (Vector2)transform.position + firePointOffset,
            Entity.Target
        );
        
        yield return new WaitUntil(() => TriggerCalled);
    }

    public override void Exit()
    {
        base.Exit();
        StopCoroutine(_currentCoroutine);
    }
}