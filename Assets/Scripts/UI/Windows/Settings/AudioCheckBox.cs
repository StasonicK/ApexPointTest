using Audio;
using Services;
using Services.PersistentProgress;
using Services.SaveLoad;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows.Settings
{
    public class AudioCheckBox : MonoBehaviour
    {
        [SerializeField] private Button _audioButton;
        [SerializeField] private Image _cancelImage;

        private bool _isOn;
        private IPlayerProgressService _playerProgressService;
        private ISaveLoadService _saveLoadService;

        public bool IsOn => _isOn;

        private void Start() =>
            _audioButton.onClick.AddListener(AudioButtonClicked);

        public void Construct()
        {
            _playerProgressService = AllServices.Container.Single<IPlayerProgressService>();
            _saveLoadService = AllServices.Container.Single<ISaveLoadService>();
            GetCheckBoxValue();
            SetAudioVolumeValue();
        }

        private void AudioButtonClicked()
        {
            _isOn = !_isOn;
            _saveLoadService.SaveAudio(_isOn);
            SetAudioVolumeValue();
        }

        private void GetCheckBoxValue() =>
            _isOn = _playerProgressService.SettingsData.IsAudioOn;

        private void SetAudioVolumeValue()
        {
            if (_isOn)
                OnAudio();
            else
                OffAudio();
        }

        private void OffAudio()
        {
            _isOn = false;
            AudioManager.Instance.Mute();
            _cancelImage.ChangeImageAlpha(Constants.MaxValue);
        }

        private void OnAudio()
        {
            _isOn = true;
            AudioManager.Instance.Unmute();
            AudioManager.Instance.ChangeVolume(_playerProgressService.SettingsData.Volume);
            _cancelImage.ChangeImageAlpha(Constants.Zero);
        }
    }
}