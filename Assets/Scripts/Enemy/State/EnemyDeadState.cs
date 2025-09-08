using UnityEngine;

public class EnemyDeadState : EnemyState
{
    public override void Enter()
    {
        base.Enter();
        Enemy.Rigidbody2D.simulated = false;
    }
}