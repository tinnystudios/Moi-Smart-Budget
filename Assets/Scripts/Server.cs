using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;

public class Server : MonoBehaviour
{
    public ResourceManager ResourceManager = new ResourceManager();

    public const string HostName = "http://moi.holepunch.io";

    public RestService RestService = new RestService();
    public ExpenseResponse ExpensesResponse;
    public BudgetResponse BudgetsResponse;

    public bool AutoStart = false;

    public bool Offline => !Online;
    public bool Online { get; private set; }

    public bool Running => RestService.Running;

    private IEnumerator Start()
    {
        if (!AutoStart)
            yield break;

        yield return UpdateDataContext();
    }

    public IEnumerator UpdateDataContext()
    {
        ResourceManager.Initialize();
        yield return HealthCheck();
        yield return GetExpenses();
        yield return GetBudgets();
    }

    public IEnumerator PostExpense(ExpenseModel expense)
    {
        if (Offline)
            ResourceManager.CacheUnsent(expense, $"expense-{DateTime.Now.ToString(CultureInfo.InvariantCulture)}.json");
        else
            yield return RestService.Post(GetApiUrl(ServerPaths.AddExpenses), expense);
    }

    public IEnumerator PostBudget(BudgetModel budget)
    {
        if (Offline)
            ResourceManager.CacheUnsent(budget, $"budget-{DateTime.Now.ToString(CultureInfo.InvariantCulture)}.json");
        else
            yield return RestService.Post(GetApiUrl(ServerPaths.AddBudgets), budget);
    }

    public IEnumerator HealthCheck()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
            Online = false;
        else
            yield return RestService.Get<List<ExpenseResponse>>(GetApiUrl(ServerPaths.GetExpenses), (resp) => Online = true, (error) => Online = false);
    }

    public IEnumerator GetExpenses()
    {
        if (Offline)
            ExpensesResponse = ResourceManager.Get<ExpenseResponse>("expenses.json"); 
        else
            yield return RestService.Get(GetApiUrl(ServerPaths.GetExpenses), ExpensesResponse, (resp) => ResourceManager.CacheResource(ExpensesResponse, "expenses.json"));

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