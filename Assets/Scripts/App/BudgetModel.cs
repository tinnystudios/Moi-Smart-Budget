using System;
using System.Collections.Generic;

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
}