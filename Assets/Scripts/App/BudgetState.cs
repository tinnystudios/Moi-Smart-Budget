using System;
using System.Collections;
using TMPro;
using System.Linq;
using UnityEngine;

public class BudgetState : MenuState, IDataBind<AccountController>, IDataBind<AddExpenseState>, ICreateState, IDataBind<DialogueBox>, IDataBind<Server>, IDataBind<Spinner>
{
    public BudgetModel BudgetModel;

    public ExpenseButton ExpenseButtonPrefab;
    public Transform Container;

    public TMP_InputField BudgetInputField;
    public TMP_InputField EndInputField;
    public TextMeshProUGUI RemainingLabel;
    public TextMeshProUGUI SpentLabel;
    public TMP_Dropdown RepeatDropDown;

    private AccountController _accountController;
    private AddExpenseState _expenseState;
    private DialogueBox _dialogueBox;
    private Server _server;
    private Spinner _spinner;

    private BudgetModel _initialBudgetModel;

    public override void Setup()
    {
        base.Setup();
        BudgetInputField.onValueChanged.AddListener(OnBudgetValueChanged);
        EndInputField.onSubmit.AddListener(OnEndDateSubmit);
        EndInputField.onSelect.AddListener(OnEndInputSelected);

        RepeatDropDown.ClearOptions();
        var options = Enum.GetNames(typeof(ERepeatType)).ToList();
        RepeatDropDown.AddOptions(options);
        RepeatDropDown.RefreshShownValue();
        RepeatDropDown.onValueChanged.AddListener(OnRepeatChanged);
    }

    public override IEnumerator TransitionIn(State state)
    {
        if (AppSettings.AutoRefresh)
        {
            _spinner.Begin();
            yield return _server.GetExpenses();
            _spinner.End();
        }

        _initialBudgetModel = new BudgetModel(BudgetModel);

        if (BudgetModel.RemainingDays <= 0 && BudgetModel.RepeatType != ERepeatType.Once)
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

        yield return base.TransitionIn(state);
    }

    public override IEnumerator TransitionOut(State state)
    {
        yield return SaveChanges();
        yield return base.TransitionOut(state);
    }

    private void OnEndInputSelected(string text)
    {
        EndInputField.text = BudgetModel.EndTime.ToShortDateString();
    }

    private void OnRepeatChanged(int value)
    {
        BudgetModel.RepeatType = (ERepeatType)value;
    }

    private void OnEndDateSubmit(string text)
    {
        BudgetModel.EndTime = DateTime.Parse(text);
        RefreshUI();
    }

    private void OnBudgetValueChanged(string text)
    {
        BudgetModel.Amount = float.Parse(text);
        RefreshUI();
    }

    public Coroutine SaveChanges()
    {
        if (_server.Running || !_initialBudgetModel.IsDifferentFrom(BudgetModel))
            return null;

        return StartCoroutine(Routine());

        IEnumerator Routine()
        {
            _spinner.Begin();
            yield return _server.UpdateBudget(BudgetModel);
            _spinner.End();
        }
    }

    public void RefreshUI()
    {
        Title = BudgetModel.Name;

        BudgetInputField.text = $"{BudgetModel.Amount}";

        var expenses = BudgetModel.Expenses;
        var sum = expenses.Sum(x => x.Cost);

        RemainingLabel.text = $"${BudgetModel.Amount - sum}";
        EndInputField.text = $"In {BudgetModel.RemainingDisplayDays} days";

        SpentLabel.text = $"${sum}";

        RepeatDropDown.SetValueWithoutNotify((int)BudgetModel.RepeatType);
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

    public void Bind(AddExpenseState data) => _expenseState = data;
    public void Bind(DialogueBox data) => _dialogueBox = data;
    public void Bind(Server data) => _server = data;
    public void Bind(Spinner data) => _spinner = data;
}

public static class AppSettings
{
    public static bool AutoRefresh = true;
}