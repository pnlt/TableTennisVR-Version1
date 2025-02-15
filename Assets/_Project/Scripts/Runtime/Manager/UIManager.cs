using TMPro;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    public TextMeshProUGUI valueDebug;

    public void SetValueDebug(string value)
    {
        valueDebug.text = value;
    }
}
