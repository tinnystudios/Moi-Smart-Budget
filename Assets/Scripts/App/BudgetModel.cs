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

    public DateTime StartDate;
    public DateTime EndDate;
    public ERepeatType RepeatType;

    public double RemainingDays => (EndDate - DateTime.Now).TotalDays;
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
        StartDate = budgetModel.StartDate;
        EndDate = budgetModel.EndDate;
        RepeatType = budgetModel.RepeatType;
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
        StartDate = EndDate;
        EndDate = StartDate.Add(TimeSpan.FromDays(RepeatDaysLookUp[RepeatType]));
    }

    public bool IsDifferentFrom(BudgetModel model)
    {
        return Id != model.Id ||
               Amount != model.Amount || 
               StartDate != model.StartDate || 
               EndDate != model.EndDate ||
               RepeatType != model.RepeatType;
    }
}
