using System.Collections;
using UnityEngine;
using System.Linq;

public class BudgetListState : MenuState, IDataBind<AccountController>, IDataBind<BudgetState>
{
    public Transform Container; 
    public BudgetButton BudgetButtonPrefab;

    private AccountController _accountController;
    private BudgetState _budgetState;

    public override IEnumerator TransitionIn(State state)
    {
        var list = Container.GetComponentsInChildren<BudgetButton>(includeInactive: true);
        foreach (var button in list.Reverse())
            Destroy(button.gameObject);

        // Generate the list
        foreach (var budget in _accountController.Budgets)
        {
            var budgetButton = Instantiate(BudgetButtonPrefab, Container);
            budgetButton.Initialize(budget);
            budgetButton.OnClick += () => 
            {
                _budgetState.UpdateModel(budget);
                _budgetState.Enter();
            };
        }

        return base.TransitionIn(state);
    }

    public void Bind(AccountController data)
    {
        _accountController = data;
    }

    public void Bind(BudgetState data)
    {
        _budgetState = data;
    }
}
