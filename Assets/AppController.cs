using UnityEngine;

public class AppController : MonoBehaviour
{
    public AccountController AccountController;
    public StateMachine StateMachine;

    private void Awake()
    {
        this.Bind<IDataBind<AccountController>, AccountController>();
        this.Bind<IDataBind<BudgetState>, BudgetState>();
        this.Bind<IDataBind<AddExpenseState>, AddExpenseState>();
        this.Bind<IDataBind<StateMachine>, StateMachine>();
        this.Bind<IDataBind<DialogueBox>, DialogueBox>();

        AccountController.Load();
        StateMachine.Begin();
    }
}