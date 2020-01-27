using System;
using System.Collections.Generic;

[Serializable]
public class ExpenseResponse : Response<List<ExpenseModel>>
{
}

[Serializable]
public class BudgetResponse : Response<List<BudgetModel>>
{
}