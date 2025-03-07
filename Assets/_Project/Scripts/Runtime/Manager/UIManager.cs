using System;
using TMPro;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    // Temporary variable and method
    public TextMeshProUGUI valueDebug;

    public void SetValueDebug(string value)
    {
        valueDebug.text = " " + value;
    }

    public void DeleteDebug()
    {
        valueDebug.text = "";
    }
}
