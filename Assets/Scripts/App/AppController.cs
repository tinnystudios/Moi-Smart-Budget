using System.Collections;
using UnityEngine;

public class AppController : MonoBehaviour, IDataBind<Server>, IDataBind<Spinner>
{
    public AccountController AccountController;
    public StateMachine StateMachine;
    private Server _server;
    private Spinner _spinner;

    private void Awake()
    {
        this.Bind<IDataBind<AccountController>, AccountController>();
        this.Bind<IDataBind<BudgetState>, BudgetState>();
        this.Bind<IDataBind<AddExpenseState>, AddExpenseState>();
        this.Bind<IDataBind<StateMachine>, StateMachine>();
        this.Bind<IDataBind<DialogueBox>, DialogueBox>();
        this.Bind<IDataBind<Server>, Server>();
        this.Bind<IDataBind<Spinner>, Spinner>();
    }

    private IEnumerator Start()
    {
        _spinner.Begin();
        yield return _server.UpdateDataContext();

        AccountController.Load(_server.BudgetsResponse.Result);
        StateMachine.Begin();

        _spinner.End();
    }

    public void Bind(Server data)
    {
        _server = data;
    }

    public void Bind(Spinner data)
    {
        _spinner = data;
    }
}