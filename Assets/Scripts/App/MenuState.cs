using System.Collections;
using UnityEngine;

public class MenuState : State, IDataBind<TitleComponent>
{
    public CanvasGroup CanvasGroup;
    public string Title = "Untitled";
    public bool Interactable;

    protected TitleComponent _titleComponent;

    public override void Setup()
    {
        SetVisibility(false);
    }

    public override IEnumerator TransitionIn(State state)
    {
        _titleComponent.SetTitle(Title, Interactable);
        SetVisibility(true);
        yield break;
    }

    public override IEnumerator TransitionOut(State state)
    {
        SetVisibility(false);
        yield break;
    }

    public void SetVisibility(bool active)
    {
        CanvasGroup.alpha = active ? 1 : 0;
        CanvasGroup.interactable = active;
        CanvasGroup.blocksRaycasts = active;
        CanvasGroup.gameObject.SetActive(active);
    }

    public void Bind(TitleComponent data)
    {
        _titleComponent = data;
    }
}