using System;
using Audio;
using GameObjects.Tank;
using Pool;
using UI.Screens.GameLoop;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows
{
    public class StartGameWindow : MonoBehaviour
    {
        [SerializeField] protected Button _playButton;
        [SerializeField] protected Animator _playButtonAnimator;

        private IObjectsMover _objectsMover;
        private IObjectsGenerator _objectsGenerator;
        private GameScreen _gameScreen;
        private TankMovement _tankMovement;
        private TankRotation _tankRotation;
        private TankShooting _tankShooting;

        public event Action PlayButtonClicked;

        public void Construct(IObjectsMover objectsMover, IObjectsGenerator objectsGenerator, GameScreen gameScreen,
            TankMovement tankMovement, TankRotation tankRotation, TankShooting tankShooting)
        {
            _tankShooting = tankShooting;
            _tankRotation = tankRotation;
            _tankMovement = tankMovement;
            _gameScreen = gameScreen;
            _objectsGenerator = objectsGenerator;
            _objectsMover = objectsMover;
        }

        public void Open()
        {
            _objectsMover.Stop();
            _objectsGenerator.Off();
            _tankRotation.Off();
            _tankMovement.Off();
            _tankShooting.Off();
            gameObject.SetActive(true);
            _playButtonAnimator.Play(Constants.AnimationButtonRotation);
            _playButton.onClick.AddListener(OnPlayButtonClick);
        }

        private void OnPlayButtonClick()
        {
            _playButton.onClick.RemoveListener(OnPlayButtonClick);
            _gameScreen.gameObject.SetActive(true);
            _objectsMover.Run();
            _objectsGenerator.On();
            _tankRotation.On();
            _tankMovement.On();
            _tankShooting.On();
            PlayButtonClicked?.Invoke();
            AudioManager.Instance.PlayAudio(AudioTrack.ClickSoundFx, false, AudioLayer.Sound);
            gameObject.SetActive(false);
        }
    }
}