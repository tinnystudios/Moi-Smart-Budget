using System.Collections;
using System.Linq;
using UnityEngine;

public class Server : MonoBehaviour
{
    public RestService RestService = new RestService();
    public ExpenseResponse ExpensesResponse;
    public BudgetResponse BudgetsResponse;

    private IEnumerator Start()
    {
        // yield return PostExpense();
        //yield return PostBudget(new BudgetModel { Name = "EatOut", Amount = 100 });

        yield return GetExpenses();
        yield return GetBudgets();
    }

    public IEnumerator PostExpense(ExpenseModel expense)
    {
        var url = "http://192.168.1.85/AddExpenses.php";
        var formData = new WWWForm();
        yield return RestService.Post(url, expense);
    }

    public IEnumerator PostBudget(BudgetModel budget)
    {
        var url = "http://192.168.1.85/AddBudgets.php";
        var formData = new WWWForm();
        yield return RestService.Post(url, budget);
    }

    public IEnumerator GetExpenses()
    {
        var url = "http://192.168.1.85/GetExpenses.php";
        yield return RestService.Get(url, ExpensesResponse);
    }

    public IEnumerator GetBudgets()
    {
        var url = "http://192.168.1.85/GetBudgets.php";
        yield return RestService.Get(url, BudgetsResponse);

        foreach (var budget in BudgetsResponse.Result)
        {
            budget.Expenses = ExpensesResponse.Result.Where(x => x.BudgetId == budget.Id).ToList();
        }
    }
}
