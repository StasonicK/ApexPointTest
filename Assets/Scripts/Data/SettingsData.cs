using System;
using Infrastructure;

namespace Data
{
    [Serializable]
    public class SettingsData
    {
        public bool IsAudioOn;
        public float Volume;

        public SettingsData(SceneId sceneId)
        {
            if (sceneId == SceneId.Main)
            {
                IsAudioOn = true;
                Volume = Constants.MaxValue;
            }
        }
    }
}