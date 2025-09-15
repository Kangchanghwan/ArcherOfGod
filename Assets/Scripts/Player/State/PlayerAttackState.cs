using System;
using System.Collections;
using UnityEngine;

public class PlayerAttackState : PlayerState
{
    private AttackBase _attackBase;
    [SerializeField] private float attackSpeed;
    [SerializeField] private Vector2 firePointOffset;
    private Coroutine _currentRoutine;
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
        FlipController();
        _currentRoutine = StartCoroutine(Attack());
    }

    private IEnumerator Attack()
    {
        yield return new WaitForSeconds(0.62f/attackSpeed);

        _attackBase.Attack(
            (Vector2)transform.position + firePointOffset,
            GameManager.Instance.PlayerOfTarget.GetTransform()
        );
        yield return new WaitUntil(() => TriggerCalled);
    }

    public override void Exit()
    {
        base.Exit();
        StopCoroutine(_currentRoutine);
    }
}