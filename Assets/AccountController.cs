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

    public void Load()
    {
        Load(ref Budgets, _fileName);
        Load(ref BudgetHistory, _historyFileName);
    }

    public void Load(ref List<BudgetModel> model, string fileName)
    {
        var path = Application.persistentDataPath;
        var fullPath = $"{path}/{fileName}";

        if (!File.Exists(fullPath))
        {
            model.Clear();
            Save(ref model, fileName);
        }

        var json = File.ReadAllText(fullPath);
        model = JsonConvert.DeserializeObject<List<BudgetModel>>(json);
    }

    public void Save()
    {
        Save(ref Budgets, _fileName);
    }

    public void Save(ref List<BudgetModel> model, string fileName)
    {
        var path = Application.persistentDataPath;
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        var fullPath = $"{path}/{fileName}";
        var json = JsonConvert.SerializeObject(model, Formatting.Indented);

        File.WriteAllText(fullPath, json);
    }

    public void NewBudgetCycle(BudgetModel budgetModel)
    {
        AddHistory(budgetModel);

        budgetModel.Expenses.Clear();
        budgetModel.NewCycle();
        Save();
    }

    public void AddHistory(BudgetModel budgetModel)
    {
        BudgetHistory.Add(budgetModel);
        Save(ref BudgetHistory, _historyFileName);
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
                Save();
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
