using Audio;
using Services;
using Services.PersistentProgress;
using Services.SaveLoad;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows.Settings
{
    public class AudioSlider : MonoBehaviour
    {
        [SerializeField] private AudioCheckBox _audioCheckBox;

        private Slider _slider;
        private IPlayerProgressService _playerProgressService;
        private ISaveLoadService _saveLoadService;

        public void Construct()
        {
            _playerProgressService = AllServices.Container.Single<IPlayerProgressService>();
            _saveLoadService = AllServices.Container.Single<ISaveLoadService>();
        }

        private void Awake()
        {
            _slider = GetComponent<Slider>();
            _slider.onValueChanged.AddListener(ChangeValue);
        }

        private void ChangeValue(float value)
        {
            if (_audioCheckBox.IsOn)
                AudioManager.Instance.ChangeVolume(value);

            Save(value);
        }

        private void OnEnable()
        {
            if (_saveLoadService != null)
                _slider.value = _playerProgressService.SettingsData.Volume;
        }

        private void Save(float value) =>
            _saveLoadService.SaveAudioVolume(value);

        private void OnDestroy()
        {
            if (_saveLoadService != null)
                _saveLoadService.SaveAllData();
        }
    }
}