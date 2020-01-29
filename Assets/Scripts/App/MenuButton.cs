using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuButton : Button
{
    public State State;

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        State.Enter();
    }
}

public abstract class Button : MonoBehaviour, IPointerClickHandler
{
    public Action OnClick;

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        OnClick?.Invoke();
    }
}
