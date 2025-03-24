using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Runtime.Enum;
using _Project.Scripts.Runtime.SImpleSaveLaodSystem;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class GameManager : PersistentSingleton<GameManager>, IDataPersistence
{
    [Header("Data")] [SerializeField] private float score;
    [SerializeField] private float coin;

    [Header("Level regulations")] [SerializeField]
    private AssetLabelReference levelDataLabel;

    [SerializeField] private List<LevelSO> levels;

    private LevelSO currentLevel;
    public int currentLevelIndex;
    private GameMode mode = GameMode.Practice;
    private List<bool> levelCompletionStatus = new();

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
        InitializationLevelStatus();
        currentLevel = levels[currentLevelIndex];
    }

    private void InitializationLevelStatus() {
        if (levelCompletionStatus == null || levelCompletionStatus.Count != levels.Count)
        {
            levelCompletionStatus = new();
            for (int i = 0; i < levels.Count; i++)
            {
                levelCompletionStatus.Add(i == 0);
            }
        }
    }

    public void CompleteCurrentLevel() {
        if (currentLevelIndex < levelCompletionStatus.Count)
        {
            levelCompletionStatus[currentLevelIndex] = true;
            if (currentLevelIndex + 1 < levelCompletionStatus.Count)
            {
                levelCompletionStatus[currentLevelIndex + 1] = true;
            }
        }

        DataPersistentManager.Instance.SaveGame();
    }

    public bool IsLevelUnlocked(int levelIdx) {
        if (levelIdx < levelCompletionStatus.Count)
        {
            return levelCompletionStatus[levelIdx];
        }

        return false;
    }

    private void UpgradeLevel() {
        currentLevelIndex++;
    }

    public void LoadData(GameData gameData) {
        score = gameData.playerCoin;
        if (gameData.levelCompletionStatus != null && gameData.levelCompletionStatus.Count > 0)
        {
            levelCompletionStatus = gameData.levelCompletionStatus;
        }
        else
        {
            InitializationLevelStatus();
        }
    }

    public void SaveData(ref GameData gameData) {
        gameData.playerCoin = score;
        gameData.levelCompletionStatus = levelCompletionStatus;
    }
}