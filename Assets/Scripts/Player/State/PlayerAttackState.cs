using UnityEngine;

public class PlayerAttackState: PlayerState
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
        Player.FlipController();
    }

    public void Attack()
    {
        attackBase.Attack(startPoint, GameManager.Instance.PlayerOfTarget.GetTransform());
    }
}
