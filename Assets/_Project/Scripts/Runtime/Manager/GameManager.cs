using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class GameManager : Singleton<GameManager>
{
    [Header("Data")]
    [SerializeField] private float score;
    [SerializeField] private float coin;

    [Header("Level regulations")] 
    [SerializeField] private AssetLabelReference levelDataLabel;
    [SerializeField] private List<LevelSO> levels;

    public LevelSO CurrentLevel { get => currentLevel; set => currentLevel = value; }
    
    private LevelSO currentLevel;
    private int currentLevelIndex;
    
    public List<LevelSO> Levels { get => levels; private set => levels = value; }

    protected override void Awake()
    {
        base.Awake();
        //LoadSOReferences();
    }

    private void LoadSOReferences()
    {
        levels = new List<LevelSO>();
        Addressables.LoadAssetsAsync<ScriptableObject>(levelDataLabel.labelString).Completed += LoadedData;
    }

    private void LoadedData(AsyncOperationHandle<IList<ScriptableObject>> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            foreach (var singleLevel in obj.Result)
            {
                levels.Add(singleLevel as LevelSO);
            }

            levels = levels.OrderBy(so => so.levelNum).ToList();
        }
    }

    /*private async void Start()
    {
        try
        {
            while (levels.Count == 0)
            {
                await UniTask.Yield();
            }
            currentLevel = levels[0];
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }*/

    private void Start()
    {
        currentLevel = levels[currentLevelIndex];
    }

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
