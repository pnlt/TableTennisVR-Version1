namespace _Project.Scripts.Runtime.SImpleSaveLaodSystem
{
    public interface IDataPersistence
    {
        void LoadData(GameData gameData);
        void SaveData(ref GameData gameData);
    }
}