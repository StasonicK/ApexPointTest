using System;
using Audio;
using GameObjects.Tank;
using Pool;
using Pool.Enemies;
using Pool.Projectiles;
using UI.Screens.GameLoop;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows
{
    public class StartGameWindow : MonoBehaviour
    {
        [SerializeField] protected Button _playButton;
        [SerializeField] protected Animator _playButtonAnimator;

        private IGameObjectsMover _gameObjectsMover;
        private GameScreen _gameScreen;
        private TankMovement _tankMovement;
        private TankRotation _tankRotation;
        private TankShooting _tankShooting;
        private TankWeaponChanger _tankWeaponChanger;
        private IEnemiesGenerator _enemiesGenerator;
        private IProjectilesGenerator _projectilesGenerator;

        public event Action PlayButtonClicked;

        public void Construct(IGameObjectsMover gameObjectsMover, IEnemiesGenerator enemiesGenerator,
            IProjectilesGenerator projectilesGenerator, GameScreen gameScreen, TankMovement tankMovement,
            TankRotation tankRotation, TankShooting tankShooting, TankWeaponChanger tankWeaponChanger)
        {
            _projectilesGenerator = projectilesGenerator;
            _enemiesGenerator = enemiesGenerator;
            _tankWeaponChanger = tankWeaponChanger;
            _tankShooting = tankShooting;
            _tankRotation = tankRotation;
            _tankMovement = tankMovement;
            _gameScreen = gameScreen;
            _gameObjectsMover = gameObjectsMover;
        }

        public void Open()
        {
            _gameObjectsMover.Stop();
            _enemiesGenerator.GameObject.SetActive(true);
            _enemiesGenerator.Off();
            _projectilesGenerator.GameObject.SetActive(true);
            _projectilesGenerator.Off();
            _tankRotation.Off();
            _tankMovement.Off();
            _tankShooting.Off();
            _tankWeaponChanger.Off();
            gameObject.SetActive(true);
            _playButtonAnimator.Play(Constants.AnimationButtonRotation);
            _playButton.onClick.AddListener(OnPlayButtonClick);
        }

        private void OnPlayButtonClick()
        {
            _playButton.onClick.RemoveListener(OnPlayButtonClick);
            _gameScreen.gameObject.SetActive(true);
            _gameObjectsMover.Run();
            _enemiesGenerator.On();
            _projectilesGenerator.On();
            _tankRotation.On();
            _tankMovement.On();
            _tankShooting.On();
            _tankWeaponChanger.On();
            PlayButtonClicked?.Invoke();
            AudioManager.Instance.PlayAudio(AudioTrack.ClickSoundFx, false, AudioLayer.Sound);
            gameObject.SetActive(false);
        }
    }
}