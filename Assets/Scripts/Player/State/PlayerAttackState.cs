using UnityEngine;

public class PlayerAttackState: PlayerState
{
    private readonly AttackBase _attackBase;
    
    private float _attackSpeed = 2f;
    
    public PlayerAttackState(PlayerContext context, string animBoolName, AttackBase attackBase) : base(context, animBoolName)
    {
        _attackBase = attackBase;
        _attackBase.Initialize(context);
    }

    public override void Enter()
    {
        base.Enter();
        Animator.SetFloat("AttackSpeed", _attackSpeed);
        Controller.FlipController();
        _attackBase.TargetRigidBody2D = 
            GameManager.Instance.PlayerOfTarget
                .GetTransform().GetComponent<Rigidbody2D>();
    }

    public void Attack()
    {
        _attackBase.Attack();
    }

    public override void Update()
    {
        base.Update();
        
        if (Controller.OnMove)
        {
            Controller.ChangeState(Controller.MoveState);
        }

        if (TriggerCalled)
        {
            Controller.ChangeState(Controller.CastingState);
        }
        
    }

}
