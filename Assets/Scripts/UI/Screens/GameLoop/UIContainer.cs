using System.Collections.Generic;
using UI.Windows;
using UnityEngine;

namespace UI.Screens.GameLoop
{
    public class UIContainer : MonoBehaviour, IUIContainer
    {
        [SerializeField] private GameScreen _gameScreen;
        [SerializeField] private StartGameWindow _startGameWindow;
        [SerializeField] private SettingsWindow _settingsWindow;
        [SerializeField] private GameOverWindow _gameOverWindow;
        [SerializeField] private FinishLevelWindow _finishLevelWindow;

        private Dictionary<WindowId, bool> _windowsStatusDictionary;

        public GameObject GameObject => gameObject;
        public StartGameWindow StartGameWindow => _startGameWindow;
        public GameOverWindow GameOverWindow => _gameOverWindow;
        public SettingsWindow SettingsWindow => _settingsWindow;
        public FinishLevelWindow FinishLevelWindow => _finishLevelWindow;
        public GameScreen GameScreen => _gameScreen;

        // public OpenSettingsButton OpenSettingsButton { get; private set; }
        // public OpenShopButton OpenShopButton { get; private set; }

        private void Awake()
        {
            DontDestroyOnLoad(this);
            // HideAll();
        }

        public void Construct()
        {
            // OpenSettingsButton = _gameScreen.OpenSettingsButton;
            // OpenShopButton = _gameScreen.OpenShopButton;
            _gameScreen.gameObject.SetActive(true);
            // OpenSettingsButton.EnableButton();
            // OpenShopButton.EnableButton();
            _windowsStatusDictionary = new();
            _windowsStatusDictionary[WindowId.StartGame] = false;
            _windowsStatusDictionary[WindowId.Finish] = false;
            _windowsStatusDictionary[WindowId.GameOver] = false;
            Unsubscribe();
            Subscribe();
        }

        private void Unsubscribe()
        {
            _startGameWindow.PlayButtonClicked -= CloseStartGameWindow;
            _gameOverWindow.RestartButtonClicked -= ShowStartGameWindow;
            _gameOverWindow.ToMainButtonClicked -= CloseGameOverWindow;
            _finishLevelWindow.NextLevel -= CloseFinishLevelWindow;
        }

        private void Subscribe()
        {
            _startGameWindow.PlayButtonClicked += CloseStartGameWindow;
            _gameOverWindow.RestartButtonClicked += ShowStartGameWindow;
            _gameOverWindow.ToMainButtonClicked += CloseGameOverWindow;
            _finishLevelWindow.NextLevel += CloseFinishLevelWindow;
        }

        public void PrepareLevel()
        {
            _windowsStatusDictionary[WindowId.StartGame] = true;
            CloseGameOverWindow();
            CloseFinishLevelWindow();
            _settingsWindow.gameObject.SetActive(false);
            ShowStartGameWindow();
        }

        public void HideAll()
        {
            _gameOverWindow.gameObject.SetActive(false);
            _startGameWindow.gameObject.SetActive(false);
            _settingsWindow.gameObject.SetActive(false);
            _finishLevelWindow.gameObject.SetActive(false);
            _gameScreen.gameObject.SetActive(false);

            // if (OpenSettingsButton != null)
            //     OpenSettingsButton.DisableButton();
            //
            // if (OpenShopButton != null)
            //     OpenShopButton.DisableButton();
        }

        public void ShowStartGameWindow()
        {
            // OpenSettingsButton.DisableButton();
            // OpenShopButton.DisableButton();

            // if (needTutorial)
            //     _gameScreen.gameObject.SetActive(false);
            // else
            _windowsStatusDictionary[WindowId.StartGame] = true;
            _gameScreen.gameObject.SetActive(false);
            _finishLevelWindow.gameObject.SetActive(false);
            _gameOverWindow.gameObject.SetActive(false);
            _startGameWindow.Open();
        }

        public void CloseStartGameWindow()
        {
            _windowsStatusDictionary[WindowId.StartGame] = false;
        }

        public void ShowFinishLevelWindow()
        {
            _windowsStatusDictionary[WindowId.Finish] = true;
            _finishLevelWindow.Open();
            // OpenSettingsButton.DisableButton();
            // OpenShopButton.DisableButton();
        }

        public void CloseFinishLevelWindow()
        {
            _windowsStatusDictionary[WindowId.Finish] = false;
            _finishLevelWindow.Close();
            // OpenSettingsButton.EnableButton();
            // OpenShopButton.EnableButton();
        }

        public void ShowGameOverWindow()
        {
            _windowsStatusDictionary[WindowId.GameOver] = true;
            _gameOverWindow.Open();
            // OpenSettingsButton.DisableButton();
            // OpenShopButton.DisableButton();
        }

        public void CloseGameOverWindow()
        {
            _windowsStatusDictionary[WindowId.GameOver] = false;
            _gameOverWindow.Close();
            // OpenSettingsButton.EnableButton();
            // OpenShopButton.EnableButton();
        }

        private bool IsOtherWindowOpen()
        {
            foreach (KeyValuePair<WindowId, bool> pair in _windowsStatusDictionary)
            {
                if (pair.Value)
                    return true;
            }

            return false;
        }
    }
}