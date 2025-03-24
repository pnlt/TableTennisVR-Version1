using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace _Project.Scripts.Runtime.SImpleSaveLaodSystem
{
    public class DataPersistentManager : PersistentSingleton<DataPersistentManager>
    {
        [Header ("File storage config")]
        [SerializeField] private string fileName;
        
        private GameData gameData;
        private List<IDataPersistence> dataPersistence;
        private FileDataHandler fileDataHandler;
        
        private void Start()
        {
            dataPersistence = FindPersistentDataObject();
            fileDataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
            LoadGame();
        }

        private List<IDataPersistence> FindPersistentDataObject()
        {
            IEnumerable<IDataPersistence> persistentDataObjects =
                FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None)
                    .OfType<IDataPersistence>();
            return new List<IDataPersistence>(persistentDataObjects);
        }

        private void NewGame()
        {
            gameData = new GameData();
        }

        public void LoadGame()
        {
            gameData = fileDataHandler.LoadDataToFile();
            if (gameData == null)
                NewGame();

            foreach (var iDataPersistence in dataPersistence)
            {
                iDataPersistence.LoadData(gameData);
            }
        }

        public void SaveGame()
        {
            Debug.LogWarning(dataPersistence.Count);
            foreach (var iDataPersistence in dataPersistence)
            {
                iDataPersistence.SaveData(ref gameData);
            }
            fileDataHandler.SaveDataToFile(gameData);
        }

        private void OnApplicationQuit()
        {
            SaveGame();
        }
    }
}