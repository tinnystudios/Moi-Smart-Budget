using System;

[Serializable]
public class ExpenseModel
{
    public int Id;
    public string Name;
    public float Cost;
    public int BudgetId;

    public DateTime PurchaseDate = DateTime.Now;
    public int UserId;

    public ExpenseModel() { }
}