using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Runtime.Enum;
using _Project.Scripts.Runtime.SImpleSaveLaodSystem;
using _Project.Scripts.Runtime.UserInterface;
using Dorkbots.XR.Runtime;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
using UnityEngine.ResourceManagement.AsyncOperations;

public class GameManager : PersistentSingleton<GameManager>, IDataPersistence
{
    [Header("Data")] [SerializeField] private float score;
    [SerializeField] private float coin;

    [Header("Level regulations")] [SerializeField]
    private AssetLabelReference levelDataLabel;

    [SerializeField] private List<LevelSO> levels;

    private LevelSO currentLevel;
    private int currentLevelIndex;
    private GameMode mode = GameMode.Practice;

    #region GameStates

    private bool _isGamePaused;

    #endregion

    public LevelSO CurrentLevel
    {
        get
        {
            if (currentLevelIndex < levels.Count)
                currentLevel = levels[currentLevelIndex];
            return currentLevel;
        }
        set => currentLevel = value;
    }

    public float NormalScore { get; set; }

    public float PlayerScore
    {
        get => score;
        set
        {
            if (score >= 0) score = value;
            else Debug.LogWarning("Score out of range");
        }
    }

    public GameMode Mode
    {
        get => mode;
        set
        {
            if (mode == GameMode.Practice && score >= currentLevel.requiredScore) mode = value;
        }
    }

    public List<LevelSO> Levels
    {
        get => levels;
        private set => levels = value;
    }

    #region Load GameManager by Addressable

    protected override void Awake() {
        base.Awake();
        //LoadSOReferences();
    }

    private void LoadSOReferences() {
        levels = new List<LevelSO>();
        Addressables.LoadAssetsAsync<ScriptableObject>(levelDataLabel.labelString).Completed += LoadedData;
    }

    private void LoadedData(AsyncOperationHandle<IList<ScriptableObject>> obj) {
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

    #endregion

    private void Start() {
        currentLevel = levels[currentLevelIndex];
    }

    private void UpgradeLevel() {
        currentLevelIndex++;
    }

    public void LoadData(GameData gameData) {
        score = gameData.playerCoin;
    }

    public void SaveData(ref GameData gameData) {
        gameData.playerCoin = ++score;
    }
}