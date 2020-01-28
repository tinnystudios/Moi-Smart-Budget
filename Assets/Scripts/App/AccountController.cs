using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AccountController : MonoBehaviour, IDataBind<StateMachine>
{
    public List<BudgetModel> Budgets;
    public List<BudgetModel> BudgetHistory;

    private string _fileName = "account.settings";
    private string _historyFileName = "history.settings";

    private StateMachine _stateMachine;

    public void Load(List<BudgetModel> budgetModels)
    {
        Budgets = budgetModels;
    }

    public void NewBudgetCycle(BudgetModel budgetModel)
    {
        AddHistory(budgetModel);

        budgetModel.Expenses.Clear();
        budgetModel.NewCycle();
    }

    public void AddHistory(BudgetModel budgetModel)
    {
        BudgetHistory.Add(budgetModel);
    }

    public void Add(BudgetModel budget)
    {
        Budgets.Add(budget);
    }

    public void Delete(BudgetModel budget)
    {
        Budgets.Remove(budget);
        AddHistory(budget);
    }

    public void Bind(StateMachine data)
    {
        _stateMachine = data;
        _stateMachine.OnStateExited += state => 
        {
            if (state is ICreateState)
            {

            }
        };
    }
}

public interface ICreateState
{

}

public class AccountModel
{

}
