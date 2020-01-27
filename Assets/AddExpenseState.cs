using TMPro;

public class AddExpenseState : MenuState, ICreateState
{
    public TMP_InputField NameInput;
    public TMP_InputField CostInput;

    private BudgetModel _budgetModel;

    public void UpdateModel(BudgetModel budgetModel)
    {
        _budgetModel = budgetModel;
    }

    public void Submit()
    {
        var expense = new ExpenseModel
        {
            Name = NameInput.text,
            Cost = float.Parse(CostInput.text),
        };

        _budgetModel.Expenses.Add(expense);
        StateMachine.Back();
    }
}
