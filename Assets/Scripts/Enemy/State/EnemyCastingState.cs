using UnityEngine;

public class EnemyCastingState: EnemyState
{
   

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Casting Enter");
    }

    public override void Exit()
    {
        base.Exit();
        Debug.Log("Casting Exit");
    }
}
