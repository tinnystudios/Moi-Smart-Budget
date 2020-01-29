using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class BudgetModel
{
    public int Id;
    public string Name = "Budget Model";
    public float Amount = 100;
    public List<ExpenseModel> Expenses = new List<ExpenseModel>();

    public DateTime StartTime;
    public DateTime EndTime;
    public ERepeatType Repeat;

    public double RemainingDays => (EndTime - DateTime.Now).TotalDays;
    public int RemainingDisplayDays => (int)RemainingDays;
        
    public float Spent => Expenses.Sum(x => x.Cost);
    public float RemainingBudget => Amount - Spent;

    public BudgetModel()
    {
    }

    public BudgetModel(BudgetModel budgetModel)
    {
        Id = budgetModel.Id;
        Amount = budgetModel.Amount;
        StartTime = budgetModel.StartTime;
        EndTime = budgetModel.EndTime;
        Repeat = budgetModel.Repeat;
    }

    public static Dictionary<ERepeatType, int> RepeatDaysLookUp = new Dictionary<ERepeatType, int>
    {
        {ERepeatType.Weekly, 7},
        {ERepeatType.Fortnightly, 14},
        {ERepeatType.Monthly, 28},
        {ERepeatType.Yearly, 365},
    };

    public void NewCycle()
    {
        StartTime = EndTime;
        EndTime = StartTime.Add(TimeSpan.FromDays(RepeatDaysLookUp[Repeat]));
    }

    public bool IsDifferentFrom(BudgetModel model)
    {
        return Id != model.Id ||
               Amount != model.Amount || 
               StartTime != model.StartTime || 
               EndTime != model.EndTime ||
               Repeat != model.Repeat;
    }
}
