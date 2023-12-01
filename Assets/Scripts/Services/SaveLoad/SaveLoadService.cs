using Data;
using Services.PersistentProgress;
using UnityEngine;

namespace Services.SaveLoad
{
    public class SaveLoadService : ISaveLoadService
    {
        private const string SettingsDataKey = "SettingsData";
        private const string GameDataKey = "GameData";

        private IPlayerProgressService _playerProgressService;

        public SaveLoadService(IPlayerProgressService playerProgressService)
        {
            _playerProgressService = playerProgressService;
        }

        public void ClearProgress()
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
        }

        public SettingsData LoadSettingsData()
        {
            string s = PlayerPrefs.GetString(SettingsDataKey);
            return s?.ToDeserialized<SettingsData>();
        }

        public GameData LoadGameData()
        {
            string s = PlayerPrefs.GetString(GameDataKey);
            return s?.ToDeserialized<GameData>();
        }

        public void ClearData()
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
        }

        public void SaveAudio(bool isOn)
        {
            _playerProgressService.SettingsData.IsAudioOn = isOn;
            PlayerPrefs.SetString(SettingsDataKey, _playerProgressService.SettingsData.ToJson());
        }

        public void SaveAudioVolume(float value)
        {
            _playerProgressService.SettingsData.Volume = value;
            PlayerPrefs.SetString(SettingsDataKey, _playerProgressService.SettingsData.ToJson());
        }

        public void SaveAllData()
        {
            PlayerPrefs.SetString(SettingsDataKey, _playerProgressService.SettingsData.ToJson());
            PlayerPrefs.SetString(GameDataKey, _playerProgressService.GameData.ToJson());
        }
    }
}