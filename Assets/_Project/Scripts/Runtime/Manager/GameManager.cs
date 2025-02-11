using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;
using VContainer;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private float score;

    public float PlayerScore
    {
        get => score;
        set
        {
            if (score >= 0) score = value;
            else Debug.LogWarning("Score out of range");
        }
    }
    
}
