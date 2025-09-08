using UnityEngine;

public class PlayerAttackState: PlayerState
{
    [SerializeField]
    private AttackBase attackBase;
    [SerializeField]
    private float attackSpeed;
    [SerializeField]
    private Transform StartPoint;


    protected override void Start()
    {
        base.Start();
        attackBase.Initialize(
            StartPoint,
            GameManager.Instance.PlayerOfTarget.GetTransform()
            );
    }
    public override void Enter()
    {
        base.Enter();
        Animator.SetFloat("AttackSpeed", attackSpeed);
        Player.FlipController();
    }

    public void Attack()
    {
        attackBase.Attack();
    }
}
