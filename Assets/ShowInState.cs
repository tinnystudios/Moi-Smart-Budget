using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ShowInState : MonoBehaviour, IDataBind<StateMachine>
{
    public List<State> States;

    public void Bind(StateMachine stateMachine)
    {
        stateMachine.OnStateEntered += state => 
        {
            var show = States.Any(x => x.gameObject == state.gameObject);
            gameObject.SetActive(show);
        };
    }
}
