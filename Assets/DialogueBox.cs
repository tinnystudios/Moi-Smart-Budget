using System;
using UnityEngine;

public class DialogueBox : MonoBehaviour
{
    public Action OnConfirm;
    public Action OnDecline;

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Confirm()
    {
        OnConfirm?.Invoke();
        Hide();
    }

    public void Decline()
    {
        OnDecline?.Invoke();
        Hide();
    }
}
