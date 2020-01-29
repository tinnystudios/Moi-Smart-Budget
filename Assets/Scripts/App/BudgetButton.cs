using TMPro;

public class BudgetButton : Button
{
    public BudgetModel Model;

    public TextMeshProUGUI NameLabel;
    public TextMeshProUGUI RemainingLabel;

    public void Initialize(BudgetModel budget)
    {
        Model = budget;
        NameLabel.text = budget.Name;
        RemainingLabel.text = $"${budget.RemainingBudget}";
    }
}
