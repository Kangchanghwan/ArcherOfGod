using UnityEngine;

public class EnemyState : MonoBehaviour
{
    [SerializeField]
    protected string animBoolName;
    
    protected Enemy Enemy;
    protected Animator Animator;
    protected Rigidbody2D Rigidbody2D;

    public bool TriggerCalled { get; private set; }

    protected virtual void Awake()
    {
        Enemy = GetComponentInParent<Enemy>();
    }

    private void EnsureInitialized()
    {
        if (Animator == null)
        {
            Animator = Enemy?.Animator;
        }

        if (Rigidbody2D == null)
        {
            Rigidbody2D =  Enemy?.Rigidbody2D;
        }
    }
    public virtual void Enter()
    {
        EnsureInitialized();
        Animator.SetBool(animBoolName, true);
        TriggerCalled = false;
        Debug.Log("Entered " + this.GetType().Name);
    }

    public virtual void Exit()
    {
        Animator.SetBool(animBoolName, false);
        Debug.Log("Exited " + this.GetType().Name);
    }


    public void AnimationTrigger() => TriggerCalled = true;


    // public override void Update()
    // {
    //     // base.Update();
    //     //
    //     //
    //     // if (CanJumpShot())
    //     // {
    //     //     skillManager.jumpShoot.SetSkillOnCooldown();
    //     //     stateMachine.ChangeState(enemy.SkillState);
    //     // }
    //     //
    // }

    // private bool CanJumpShot()
    // {
    //     if (skillManager.jumpShoot.CanUseSkill() == false)
    //     {
    //         return false;
    //     }
    //     if (enemy.stateMachine.currentState == enemy.SkillState)
    //     {
    //         return false;
    //     }
    //     if (enemy.stateMachine.currentState == enemy.IdleState)
    //     {
    //         return false;
    //     }
    //     if (enemy.stateMachine.currentState == enemy.DeadState)
    //     {
    //         return false;
    //     }
    //     return true;
    // }
    //
    // protected void FaceTarget()
    // {
    //     if (enemy.target != null)
    //     {
    //         Vector3 directionToEnemy = enemy.target.GetTransform().position - enemy.transform.position;
    //         if (directionToEnemy.x > 0 && !enemy.facingRight)
    //             enemy.Flip();
    //         else if (directionToEnemy.x < 0 && enemy.facingRight)
    //             enemy.Flip();
    //     }
    // }

    public virtual void StateUpdate()
    {
    }
}