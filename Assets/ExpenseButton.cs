using TMPro;

public class ExpenseButton : Button
{
    public ExpenseModel Model;

    public TextMeshProUGUI Name;
    public TextMeshProUGUI Date;
    public TextMeshProUGUI Cost;

    public void Initialize(ExpenseModel model)
    {
        Model = model;
        Name.text = model.Name;

        var date = model.PurchaseDate;
        Date.text = $"{date.Day}/{date.Month}/{date.Year.ToString().Substring(2,2)}";
        Cost.text = $"${model.Cost}";
    }
}