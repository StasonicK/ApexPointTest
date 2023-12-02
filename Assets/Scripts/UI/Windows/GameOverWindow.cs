using System;
using Audio;
using GameObjects.Tank;
using Infrastructure;
using Infrastructure.States;
using Pool;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows
{
    public class GameOverWindow : MonoBehaviour
    {
        [SerializeField] protected Button _restartButton;
        [SerializeField] protected Button _toMainButton;

        private IGameObjectsGenerator _gameObjectsGenerator;
        private TankMovement _tankMovement;
        private TankRotation _tankRotation;
        private TankShooting _tankShooting;
        private IGameStateMachine _gameStateMachine;

        public event Action RestartButtonClicked;
        public event Action ToMainButtonClicked;

        public void Construct(IGameObjectsGenerator gameObjectsGenerator, TankMovement tankMovement,
            TankRotation tankRotation,
            TankShooting tankShooting, IGameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
            _tankShooting = tankShooting;
            _tankRotation = tankRotation;
            _tankMovement = tankMovement;
            _gameObjectsGenerator = gameObjectsGenerator;
        }

        public void Open()
        {
            gameObject.SetActive(true);
            _restartButton.onClick.AddListener(OnRestartButtonClick);
            _toMainButton.onClick.AddListener(OnToMainButtonClick);
            AudioManager.Instance.PlayAudio(AudioTrack.DeathSoundFx, false, AudioLayer.Sound);
        }

        public void Close()
        {
            _restartButton.onClick.RemoveListener(OnRestartButtonClick);
            _toMainButton.onClick.RemoveListener(OnToMainButtonClick);
            gameObject.SetActive(false);
        }

        private void OnRestartButtonClick()
        {
            _tankMovement.Off();
            _tankRotation.Off();
            _tankShooting.Off();
            _gameObjectsGenerator.Reset();
            RestartButtonClicked?.Invoke();
            Close();
        }

        private void OnToMainButtonClick()
        {
            _tankMovement.Off();
            _tankRotation.Off();
            _tankShooting.Off();
            _gameObjectsGenerator.Reset();
            ToMainButtonClicked?.Invoke();
            Close();
            _gameStateMachine.Enter<LoadSceneState, SceneId>(SceneId.Main);
        }
    }
}