using TMPro;
using UnityEngine;

public class TitleComponent : MonoBehaviour
{
    public TextMeshProUGUI Label;

    public void SetTitle(string text)
    {
        Label.text = text;
    }
}
