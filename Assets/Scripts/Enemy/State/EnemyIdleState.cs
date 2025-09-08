using UnityEngine;

public class EnemyIdleState : EnemyState
{
    public override void Enter()
    {
        base.Enter();
        Enemy.Rigidbody2D.simulated = false;
    }
}