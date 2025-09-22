using Unity.VisualScripting;
using UnityEngine;

public abstract class StateMachineBase : MonoBehaviour
{
    protected StateBase CurrentState { get; private set; }


    protected virtual void Update()
    {
        if (CurrentState == null) return;
        
        HandleStateTransitions();
        CurrentState.StateUpdate();
    }

    protected void Initialize(StateBase startState)
    {
        CurrentState = startState;
        CurrentState.Enter();
    }

    protected void ChangeState(StateBase newState)
    {
        CurrentState.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }

    public void AnimationTrigger()
    {
        CurrentState.AnimationTrigger();
    }

    protected abstract void HandleStateTransitions();
}