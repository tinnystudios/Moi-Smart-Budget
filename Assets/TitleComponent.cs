using TMPro;
using UnityEngine;

public class TitleComponent : MonoBehaviour
{
    public TextMeshProUGUI Label;
    public TMP_InputField InputField;

    public void SetTitle(string text, bool interactable = false)
    {
        Label.text = text;
        InputField.text = text;
        InputField.interactable = interactable;
    }
}
