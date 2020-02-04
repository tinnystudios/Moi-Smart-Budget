using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Server : MonoBehaviour
{
    public ResourceManager ResourceManager = new ResourceManager();

    public const string HostName = "http://13.211.83.187:3000";

    public RestService RestService = new RestService();
    public ExpenseResponse ExpensesResponse;
    public BudgetResponse BudgetsResponse;

    public bool AutoStart = false;

    public bool Offline => !Online;
    public bool Online { get; private set; }

    public bool Running => RestService.Running;
    public string Now => DateTime.Now.ToString(CultureInfo.InvariantCulture).Replace("/", "-").Replace(" ", "-").Replace(":", "-");

    private IEnumerator Start()
    {
        StartCoroutine(HealthIntervalCheck());

        if (!AutoStart)
            yield break;

        yield return UpdateDataContext();
    }

    private IEnumerator HealthIntervalCheck()
    {
        while (true)
        {
            yield return new WaitForSeconds(2);
            yield return HealthCheck();
        }
    }

    public IEnumerator UpdateDataContext()
    {
        ResourceManager.Initialize();
        yield return HealthCheck();
        yield return GetExpenses();
        yield return GetBudgets();
        yield return UpdateOutOfDateBudgets();
    }

    public IEnumerator UpdateOutOfDateBudgets()
    {
        bool has = false;
        foreach (var budget in BudgetsResponse.Result)
        {
            if (budget.RemainingDays <= 1 && budget.RepeatType != ERepeatType.Once)
            {
                foreach (var expense in budget.Expenses)
                {
                    yield return PostHistory(expense);
                    yield return DeleteExpense(expense);
                    has = true;
                }

                budget.NewCycle();
                yield return UpdateBudget(budget);
            }
        }

        if (has)
        {
            yield return GetExpenses();
            yield return GetBudgets();
        }
    }

    public void Refresh()
    {
        SceneManager.LoadScene(0);
        // At the moment, I've chosen to reload the entire scene as reloading data also re-creating the visuals.
    }

    public IEnumerator HealthCheck()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
            Online = false;
        else
            yield return RestService.Get<HealthResponse>(GetApiUrl(ServerPaths.GetHealth), (resp) => Online = true, (error) => Online = false);
    }

    public class HealthResponse
    {
        public string Status;
    }

    public IEnumerator PostExpense(ExpenseModel expense)
    {
        if (Offline)
        {
            // For the server to upload
            ResourceManager.CacheUnsent(expense, $"expense-{Now}.json");

            // The current local app need to also cache it again
            ExpensesResponse.Result.Add(expense);
            ResourceManager.CacheResource(ExpensesResponse, "expenses.json");
        }
        else
            yield return RestService.Post(GetApiUrl(ServerPaths.AddExpenses), expense);
    }

    public IEnumerator PostHistory(ExpenseModel expense)
    {
        yield return RestService.Post(GetApiUrl(ServerPaths.AddHistories), expense);
    }

    public IEnumerator DeleteExpense(ExpenseModel expense)
    {
        yield return RestService.Post(GetApiUrl(ServerPaths.DeleteExpense), expense);
    }

    /// <summary>
    /// Making a new budget
    /// </summary>
    /// <returns></returns>
    public IEnumerator PostBudget(BudgetModel budget)
    {
        if (Offline)
        {
            ResourceManager.CacheUnsent(budget, $"budget-{Now}.json");
            budget.Id = BudgetsResponse.Result.Count + 1;
            ResourceManager.CacheResource(BudgetsResponse, "budgets.json");
        }
        else
        {
            yield return RestService.Post(GetApiUrl(ServerPaths.AddBudgets), budget);
            yield return GetBudgets();
            UnityEngine.GameObject.FindObjectOfType<AccountController>().Load(BudgetsResponse.Result);
        }
    }

    public IEnumerator GetExpenses()
    {
        if (Offline)
            ExpensesResponse = ResourceManager.Get<ExpenseResponse>("expenses.json");
        else
        {
            yield return RestService.Get(GetApiUrl(ServerPaths.GetExpenses), ExpensesResponse, (resp) =>
            {
                if (ExpensesResponse?.Result == null)
                    ExpensesResponse = new ExpenseResponse() {Result = new List<ExpenseModel>()};

                ResourceManager.CacheResource(ExpensesResponse, "expenses.json");
            });
        }

        RefreshBudgetExpenseList();
    }

    public IEnumerator GetBudgets()
    {
        if (Offline)
            BudgetsResponse = ResourceManager.Get<BudgetResponse>("budgets.json");
        else
            yield return RestService.Get(GetApiUrl(ServerPaths.GetBudgets), BudgetsResponse, (resp) => ResourceManager.CacheResource(BudgetsResponse, "budgets.json"));

        RefreshBudgetExpenseList();
    }

    public IEnumerator UpdateBudget(BudgetModel budgetModel)
    {
        yield return RestService.Post(GetApiUrl(ServerPaths.UpdateBudget), budgetModel);
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