using Interface;
using UnityEngine.Scripting;

namespace MVC.Controller
{
    [Preserve]
    public class StateMachine
    {
        public IState CurrentState { get; private set; }

        public void Initialize(IState startState)
        {
            CurrentState = startState;
            CurrentState.Enter();
        }

        public void ChangeState(IState newState)
        {
            CurrentState.Exit();
            CurrentState = newState;
            CurrentState.Enter();
        }
    }
}