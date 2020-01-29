using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public Action<State> OnStateEntered;
    public Action<State> OnStateExited;

    public State Initial;
    public State Current;

    private List<State> _states = new List<State>();

    public void Begin()
    {
        var states = GetComponentsInChildren<State>(includeInactive: true);
        foreach (var state in states)
        {
            state.Setup();
            state.Initialize(this);
        }

        GoToState(Initial);
    }

    public Coroutine GoToState(State state)
    {
        return StartCoroutine(GoTo(state));
    }

    public IEnumerator GoTo(State state)
    {
        if (state == Current)
            yield break;

        // TODO Run Pre State Scripts

        if (Current != null)
        {
            yield return Current.TransitionOut(state);
            OnStateExited?.Invoke(Current);
        }

        yield return state.TransitionIn(Current);

        // TODO Run Post State Scripts

        Current = state;
        OnStateEntered?.Invoke(Current);
        _states.Add(Current);
    }

    [ContextMenu("Back")]
    public void Back()
    {
        if (_states.Count == 1)
            return;

        StartCoroutine(Routine());

        IEnumerator Routine()
        {
            var previous = _states[_states.Count - 2];

            yield return Current.TransitionOut(previous);
            OnStateExited?.Invoke(Current);

            yield return previous.TransitionIn(Current);
            OnStateEntered?.Invoke(previous);

            _states.Remove(Current);
            Current = previous;
        }
    }
}
