using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;

public class AddBudgetState : MenuState, IDataBind<AccountController>, ICreateState, IDataBind<Server>, IDataBind<Spinner>
{
    public TMP_InputField NameInput;
    public TMP_InputField BudgetInput;
    public TMP_InputField StartDateInput;
    public TMP_InputField EndDateInput;
    public TMP_Dropdown RepeatDropDown;

    private AccountController _accountController;
    private Server _server;
    private Spinner _spinner;

    public override void Setup()
    {
        base.Setup();
        RepeatDropDown.ClearOptions();
        var options = Enum.GetNames(typeof(ERepeatType)).ToList();
        RepeatDropDown.AddOptions(options);
        RepeatDropDown.RefreshShownValue();
    }

    public override IEnumerator TransitionIn(State state)
    {
        StartDateInput.text = DateTime.Now.ToShortDateString();
        EndDateInput.text = DateTime.Now.AddDays(7).ToShortDateString();

        return base.TransitionIn(state);
    }

    public void Bind(AccountController data)
    {
        _accountController = data;
    }

    public void Submit()
    {
        if (_server.Running)
            return;

        StartCoroutine(Routine());

        IEnumerator Routine()
        {
            var endTime = DateTime.Now;
            var startTime = DateTime.Parse(StartDateInput.text);
            var repeatType = (ERepeatType)RepeatDropDown.value;

            if (repeatType != ERepeatType.Once)
                endTime = startTime.Add(TimeSpan.FromDays(BudgetModel.RepeatDaysLookUp[repeatType]));

            var budget = new BudgetModel
            {
                Name = NameInput.text,
                Amount = float.Parse(BudgetInput.text),
                StartTime = startTime,
                Repeat = repeatType,
                EndTime = endTime,
            };

            _spinner.Begin();
            yield return _server.PostBudget(budget);
            _spinner.End();

            _accountController.Add(budget);
            StateMachine.Back();
        }
    }

    public void Bind(Server data)
    {
        _server = data;
    }

    public void Bind(Spinner data)
    {
        _spinner = data;
    }
}