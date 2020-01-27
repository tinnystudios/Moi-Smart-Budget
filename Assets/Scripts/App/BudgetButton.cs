using TMPro;

public class BudgetButton : Button
{
    public BudgetModel Model;

    public TextMeshProUGUI Label;

    public void Initialize(BudgetModel budget)
    {
        Model = budget;
        Label.text = budget.Name;
    }
}
