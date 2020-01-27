using System.Collections;
using UnityEngine;

public class MenuState : State
{
    public CanvasGroup CanvasGroup;

    public override void Setup()
    {
        SetVisibility(false);
    }

    public override IEnumerator TransitionIn(State state)
    {
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
}