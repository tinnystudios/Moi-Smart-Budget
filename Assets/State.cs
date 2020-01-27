using System.Collections;
using UnityEngine;

public abstract class State : MonoBehaviour
{
    protected StateMachine StateMachine;

    public abstract void Setup();
    public abstract IEnumerator TransitionIn(State state);
    public abstract IEnumerator TransitionOut(State state);

    public void Initialize(StateMachine stateMachine)
    {
        StateMachine = stateMachine;
    }

    public void Enter()
    {
        StateMachine.GoToState(this);
    }
}
