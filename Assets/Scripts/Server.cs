using System.Collections;
using System.Linq;
using UnityEngine;

public class ServerPaths
{
    public const string GetExpenses = "GetExpenses.php";
    public const string AddExpenses = "AddExpenses.php";
    public const string GetBudgets = "GetBudgets.php";
    public const string AddBudgets = "AddBudgets.php";
}

public class Server : MonoBehaviour
{
    public const string HostName = "http://moi.holepunch.io";

    public RestService RestService = new RestService();
    public ExpenseResponse ExpensesResponse;
    public BudgetResponse BudgetsResponse;

    public bool AutoStart = false;

    public bool Running => RestService.Running;

    private IEnumerator Start()
    {
        if (!AutoStart)
            yield break;

        yield return GetExpenses();
        yield return GetBudgets();
    }

    public IEnumerator UpdateDataContext()
    {
        yield return GetExpenses();
        yield return GetBudgets();
    }

    public IEnumerator PostExpense(ExpenseModel expense)
    {
        yield return RestService.Post(GetApiUrl(ServerPaths.AddExpenses), expense);
    }

    public IEnumerator PostBudget(BudgetModel budget)
    {
        yield return RestService.Post(GetApiUrl(ServerPaths.AddBudgets), budget);
    }

    public IEnumerator GetExpenses()
    {
        yield return RestService.Get(GetApiUrl(ServerPaths.GetExpenses), ExpensesResponse);
        RefreshBudgetExpenseList();
    }

    public IEnumerator GetBudgets()
    {
        yield return RestService.Get(GetApiUrl(ServerPaths.GetBudgets), BudgetsResponse);
        RefreshBudgetExpenseList();
    }

    public void RefreshBudgetExpenseList()
    {
        foreach (var budget in BudgetsResponse.Result)
            budget.Expenses = ExpensesResponse.Result.Where(x => x.BudgetId == budget.Id).ToList();
    }

    public string GetApiUrl(string api, params string[] parameters)
    {
        return $"{HostName}/{api}";
    }
}
