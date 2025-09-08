using UnityEngine;

public class EnemyAttackState : EnemyState
{
    [SerializeField]
    private AttackBase attackBase;
    [SerializeField]
    private float attackSpeed;
    [SerializeField]
    private Transform startPoint;
    

    public override void Enter()
    {
        base.Enter();
        Animator.SetFloat("AttackSpeed", attackSpeed);
        Enemy.FlipController();
    }

    public void Attack()
    {
        attackBase.Attack(startPoint, GameManager.Instance.EnemyOfTarget.GetTransform());
    }
}
