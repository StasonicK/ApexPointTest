using System;
using Pool;
using Services;
using Services.SaveLoad;
using UI.Windows.Settings;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows
{
    public class SettingsWindow : MonoBehaviour
    {
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _restartButton;
        [SerializeField] private AudioCheckBox _audioCheckBox;
        [SerializeField] private AudioSlider _audioSlider;

        private ISaveLoadService _saveLoadService;
        private IGameObjectsGenerator _gameObjectsGenerator;

        public event Action CloseButtonClicked;
        public event Action RestartButtonClicked;

        public void Construct(IGameObjectsGenerator gameObjectsGenerator)
        {
            _gameObjectsGenerator = gameObjectsGenerator;
            _saveLoadService = AllServices.Container.Single<ISaveLoadService>();
            _audioCheckBox.Construct();
            _audioSlider.Construct();
        }

        public void Open()
        {
            gameObject.SetActive(true);
            _closeButton.onClick.AddListener(OnCloseButtonClicked);
            _restartButton.onClick.AddListener(OnRestartButtonClicked);
        }

        public void Close(bool animate = true)
        {
            _closeButton.onClick.RemoveListener(OnCloseButtonClicked);
            _restartButton.onClick.RemoveListener(OnRestartButtonClicked);
            gameObject.SetActive(false);
        }

        private void OnCloseButtonClicked()
        {
            Save();
            CloseButtonClicked?.Invoke();
        }

        private void OnRestartButtonClicked()
        {
            Save();
            RestartButtonClicked?.Invoke();
        }

        private void Save() =>
            _saveLoadService.SaveAudio(_audioCheckBox.IsOn);

        private void OnDestroy()
        {
            if (_saveLoadService != null)
                _saveLoadService.SaveAllData();
        }
    }
}