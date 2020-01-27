using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.Linq;
using UnityEngine;

public class BudgetState : MenuState, IDataBind<AccountController>, IDataBind<AddExpenseState>, ICreateState, IDataBind<DialogueBox>
{
    public BudgetModel BudgetModel;

    public ExpenseButton ExpenseButtonPrefab;
    public Transform Container;

    public TextMeshProUGUI TitleLabel;

    public TMP_InputField BudgetInputField;
    public TextMeshProUGUI RemainingLabel;
    public TextMeshProUGUI EndLabel;
    public TextMeshProUGUI SpentLabel;

    private AccountController _accountController;
    private AddExpenseState _expenseState;
    private DialogueBox _dialogueBox;

    public override void Setup()
    {
        base.Setup();
        BudgetInputField.onValueChanged.AddListener(OnBudgetValueChanged);
    }

    private void OnBudgetValueChanged(string text)
    {
        BudgetModel.Amount = float.Parse(text);
        _accountController.Save();
        RefreshUI();
    }

    public override IEnumerator TransitionIn(State state)
    {
        if (BudgetModel.RemainingDays <= 0 && BudgetModel.Repeat != ERepeatType.Once)
            _accountController.NewBudgetCycle(BudgetModel);

        RefreshUI();

        var list = Container.GetComponentsInChildren<ExpenseButton>(includeInactive: true);
        foreach (var button in list.Reverse())
            Destroy(button.gameObject);

        foreach (var expense in BudgetModel.Expenses)
        {
            var expenseButton = Instantiate(ExpenseButtonPrefab, Container);
            expenseButton.Initialize(expense);
        }

        return base.TransitionIn(state);
    }

    public void RefreshUI()
    {
        TitleLabel.text = BudgetModel.Name;
        BudgetInputField.text = $"{BudgetModel.Amount}";

        var expenses = BudgetModel.Expenses;
        var sum = expenses.Sum(x => x.Cost);

        RemainingLabel.text = $"${BudgetModel.Amount - sum}";
        EndLabel.text = $"In {BudgetModel.RemainingDisplayDays} days";

        SpentLabel.text = $"${sum}";
    }

    public void Bind(AccountController data)
    {
        _accountController = data;
    }

    public void UpdateModel(BudgetModel budget)
    {
        BudgetModel = budget;
    }

    public void NewExpense()
    {
        _expenseState.UpdateModel(BudgetModel);
        _expenseState.Enter();
    }

    public void Delete()
    {
        _dialogueBox.Show();
        _dialogueBox.OnConfirm += ActuallyDelete;

        void ActuallyDelete()
        {
            _accountController.Delete(BudgetModel);
            StateMachine.Back();
            _dialogueBox.OnConfirm -= ActuallyDelete;
        }
        
    }

    public void Bind(AddExpenseState data)
    {
        _expenseState = data;
    }

    public void Bind(DialogueBox data)
    {
        _dialogueBox = data;
    }
}

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


[Serializable]
public class ExpenseModel
{
    public string Name;
    public float Cost;
    public int BudgetId;

    public DateTime PurchaseDate = DateTime.Now;

    public ExpenseModel() { }
}