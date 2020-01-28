using System.Collections;
using TMPro;

public class AddExpenseState : MenuState, ICreateState, IDataBind<Server>, IDataBind<Spinner>
{
    public TMP_InputField NameInput;
    public TMP_InputField CostInput;

    private BudgetModel _budgetModel;
    private Server _server;
    private Spinner _spinner;

    public void UpdateModel(BudgetModel budgetModel)
    {
        _budgetModel = budgetModel;
    }

    public void Submit()
    {
        if (_server.Running)
            return;

        StartCoroutine(Routine());
        
        IEnumerator Routine()
        {
            var expense = new ExpenseModel
            {
                BudgetId = _budgetModel.Id,
                Name = NameInput.text,
                Cost = float.Parse(CostInput.text),
            };

            _budgetModel.Expenses.Add(expense);

            _spinner.Begin();
            yield return _server.PostExpense(expense);
            _spinner.End();

            StateMachine.Back();
        }
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
