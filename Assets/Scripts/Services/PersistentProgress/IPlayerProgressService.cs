using Data;

namespace Services.PersistentProgress
{
    public interface IPlayerProgressService : IService
    {
        public SettingsData SettingsData { get; }
        public GameData GameData { get; }
        void ClearAllData();
        void SetNewData(SettingsData settingsData, GameData gameData);
        void SetSettingsData(SettingsData settingsData);
        void SetGameData(GameData gameData);
        void SaveMoneyCount(int score);
    }
}