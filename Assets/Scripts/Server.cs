using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
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
        yield return RestService.Post(GetApiUrl(ServerPaths.AddExpenses), expense);
    }

    public IEnumerator PostBudget(BudgetModel budget)
    {
        yield return RestService.Post(GetApiUrl(ServerPaths.AddBudgets), budget);
    }

    public IEnumerator HealthCheck()
    {
        yield return RestService.Get<List<ExpenseResponse>>(GetApiUrl(ServerPaths.GetExpenses), 
            (resp) => Online = true,
            (error) => Online = false);
    }

    public IEnumerator GetExpenses()
    {
        if (Offline)
            ExpensesResponse = ResourceManager.Get<ExpenseResponse>("expenses.json"); 
        else
            yield return RestService.Get(GetApiUrl(ServerPaths.GetExpenses), ExpensesResponse, (resp) => ResourceManager.Cache(ExpensesResponse, "expenses.json"));

        RefreshBudgetExpenseList();
    }

    public IEnumerator GetBudgets()
    {
        if (Offline)
            BudgetsResponse = ResourceManager.Get<BudgetResponse>("budgets.json");
        else
            yield return RestService.Get(GetApiUrl(ServerPaths.GetBudgets), BudgetsResponse, (resp) => ResourceManager.Cache(BudgetsResponse, "budgets.json"));

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

public class ResourceManager
{
    public string CachePath;

    public void Initialize()
    {
        CachePath = $"{Application.persistentDataPath}/Resources";

        if (!Directory.Exists(CachePath))
            Directory.CreateDirectory(CachePath);
    }

    public void Cache(object obj, string fileName)
    {
        var json = JsonConvert.SerializeObject(obj, Formatting.Indented);
        Cache(json, fileName);
    }

    public void Cache(string json, string fileName)
    {
        File.WriteAllText(FullPath(fileName), json);
    }

    public bool HasCache(string fileName)
    {
        return File.Exists(FullPath(fileName));
    }

    public T Get<T>(string fileName)
    {
        var json = File.ReadAllText(FullPath(fileName));
        return JsonConvert.DeserializeObject<T>(json);
    }

    public string FullPath(string fileName) => $"{CachePath}/{fileName}";
}