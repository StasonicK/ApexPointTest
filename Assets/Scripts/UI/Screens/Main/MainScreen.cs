using Audio;
using Infrastructure;
using Infrastructure.States;
using Services;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Screens.Main
{
    public class MainScreen : MonoBehaviour
    {
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _exitButton;

        private IGameStateMachine _gameStateMachine;

        private void OnEnable()
        {
            _playButton.onClick.AddListener(OnPlayButtonClick);
            _exitButton.onClick.AddListener(OnExitButtonClick);

            if (_gameStateMachine == null)
                _gameStateMachine = AllServices.Container.Single<IGameStateMachine>();
        }

        private void OnDisable()
        {
            _playButton.onClick.RemoveListener(OnPlayButtonClick);
            _exitButton.onClick.RemoveListener(OnExitButtonClick);
        }

        private void OnPlayButtonClick()
        {
            AudioManager.Instance.PlayAudio(AudioTrack.ClickSoundFx, false, AudioLayer.Sound);
            _gameStateMachine.Enter<LoadSceneState, SceneId>(SceneId.Level_1);
        }

        private void OnExitButtonClick() =>
            Application.Quit();
    }
}