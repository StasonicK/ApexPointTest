using Data;

namespace Services.SaveLoad
{
    public interface ISaveLoadService : IService
    {
        void ClearData();
        void SaveAudio(bool isOn);
        void SaveAudioVolume(float value);
        void SaveAllData();
        void ClearProgress();
        SettingsData LoadSettingsData();
        GameData LoadGameData();
    }
}