using UnityEngine;
using UnityEngine.UI;

public class TestCheckUI : MonoBehaviour
{
    public Image checkImage;
    private static int _id;

    public int ID;

    private void Awake()
    {
        _id++;
        ID = _id;
        checkImage = GetComponent<Image>();   
    }

    public void ChangeColor(Color color)
    {
        checkImage.color = color;
    }
}
