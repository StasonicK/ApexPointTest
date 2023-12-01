using Data;
using UnityEngine;

namespace Services.PersistentProgress
{
    public class PlayerProgressService : IPlayerProgressService
    {
        public SettingsData SettingsData { get; private set; }
        public GameData GameData { get; private set; }

        public void ClearAllData()
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
        }


        public void SetNewData(SettingsData settingsData, GameData gameData)
        {
            SettingsData = settingsData;
            GameData = gameData;
        }

        public void SetSettingsData(SettingsData settingsData) =>
            SettingsData = settingsData;

        public void SetGameData(GameData gameData) =>
            GameData = gameData;

        public void SaveMoneyCount(int score)
        {
            // GameData.
            // GameData.MoneyCount = score;
        }
    }
}