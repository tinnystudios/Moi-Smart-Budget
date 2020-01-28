using System.Collections;
using UnityEngine;

public class AppController : MonoBehaviour, IDataBind<Server>
{
    public AccountController AccountController;
    public StateMachine StateMachine;
    private Server _server;

    private void Awake()
    {
        this.Bind<IDataBind<AccountController>, AccountController>();
        this.Bind<IDataBind<BudgetState>, BudgetState>();
        this.Bind<IDataBind<AddExpenseState>, AddExpenseState>();
        this.Bind<IDataBind<StateMachine>, StateMachine>();
        this.Bind<IDataBind<DialogueBox>, DialogueBox>();
        this.Bind<IDataBind<Server>, Server>();
    }

    private IEnumerator Start()
    {
        yield return _server.UpdateDataContext();

        AccountController.Load(_server.BudgetsResponse.Result);
        StateMachine.Begin();
    }

    public void Bind(Server data)
    {
        _server = data;
    }
}