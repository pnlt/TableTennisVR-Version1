using System;
using System.IO;
using UnityEngine;

namespace _Project.Scripts.Runtime.SImpleSaveLaodSystem
{
    public class FileDataHandler
    {
        private string filePath;
        private string fileName;

        public FileDataHandler(string filePath, string fileName)
        {
            this.filePath = filePath;
            this.fileName = fileName;
        }

        public GameData LoadDataToFile()
        {
            string fullPath = Path.Combine(filePath, fileName);
            GameData gameData = null;

            if (File.Exists(fullPath))
            {
                try
                {
                    string storedData = "";
                    using FileStream fileStream = new FileStream(fullPath, FileMode.Open);
                    using StreamReader streamReader = new StreamReader(fileStream);
                    storedData = streamReader.ReadToEnd();
                    gameData = JsonUtility.FromJson<GameData>(storedData);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }

            return gameData;
        }

        public void SaveDataToFile(GameData gameData)
        {
            string fullPath = Path.Combine(filePath, fileName);
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath)!);

                string dataToStore = JsonUtility.ToJson(gameData, true);
                Debug.Log(dataToStore + " saved");

                using FileStream fileStream = new FileStream(fullPath, FileMode.Create);
                using StreamWriter streamWriter = new StreamWriter(fileStream);
                streamWriter.Write(dataToStore);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}