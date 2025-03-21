using System;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    private GameManager gameManager;
    private void Awake()
    {
        gameManager = GameManager.Instance;
        InitializeData();
        
    }

    private void InitializeData()
    {
        PlayerPrefs.SetFloat("NormalScore", gameManager.NormalScore);
    }
}

