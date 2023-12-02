using System;
using Pool;
using Services;
using Services.PersistentProgress;
using UI.Windows;
using UnityEngine;

namespace GameObjects.Tank
{
    public class TankHealth : MonoBehaviour, IHealth
    {
        [SerializeField] private float _healthCount;

        private IPlayerProgressService _playerProgressService;
        private TankMovement _tankMovement;
        private TankDeathVfx _tankDeathVfx;
        private GameOverWindow _gameOverWindow;
        private TankHitVfx _tankHitVfx;
        private IGameObjectsMover _gameObjectsMover;
        private float _armor;

        public float Max { get; private set; }
        public float Current { get; private set; }

        public event Action HealthChanged;

        public void Construct(TankMovement tankMovement, TankDeathVfx tankDeathVfx, TankHitVfx tankHitVfx,
            IGameObjectsMover gameObjectsMover, GameOverWindow gameOverWindow)
        {
            _gameObjectsMover = gameObjectsMover;
            _tankHitVfx = tankHitVfx;
            _gameOverWindow = gameOverWindow;
            _tankDeathVfx = tankDeathVfx;
            _tankMovement = tankMovement;
            _playerProgressService = AllServices.Container.Single<IPlayerProgressService>();
            Max = _playerProgressService.GameData.HealthCount;
            Current = _playerProgressService.GameData.HealthCount;
            _armor = _playerProgressService.GameData.Armor;
        }

        private void Die()
        {
            _tankDeathVfx.Show();
            _tankMovement.Off();
            _gameObjectsMover.Stop();
            _gameOverWindow.Open();
        }

        public void GetHit(float damage)
        {
            Current = _playerProgressService.GameData.TempHealthCount -= damage * _armor;

            if (Current > 0f)
                _playerProgressService.GameData.RefreshHealth(_healthCount);
            else
                Die();

            _tankHitVfx.Show();
            HealthChanged?.Invoke();
        }
    }
}