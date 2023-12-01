﻿using Pool;
using Services;
using Services.PersistentProgress;
using UI.Windows;
using UnityEngine;

namespace GameObjects.Tank
{
    public class TankHealth : MonoBehaviour
    {
        [SerializeField] private int _healthCount;

        private IPlayerProgressService _playerProgressService;
        private TankMovement _tankMovement;
        private TankDeath _tankDeath;
        private GameOverWindow _gameOverWindow;
        private TankHit _tankHit;
        private IObjectsMover _objectsMover;

        public void Construct(TankMovement tankMovement, TankDeath tankDeath,
            TankHit tankHit, IObjectsMover objectsMover,
            GameOverWindow gameOverWindow)
        {
            _objectsMover = objectsMover;
            _tankHit = tankHit;
            _gameOverWindow = gameOverWindow;
            _tankDeath = tankDeath;
            _tankMovement = tankMovement;
            _playerProgressService = AllServices.Container.Single<IPlayerProgressService>();
            _healthCount = _playerProgressService.GameData.HealthCount;
        }

        public void Reduce()
        {
            _playerProgressService.GameData.ReduceLifes();

            if (_playerProgressService.GameData.TempHealthCount == 0)
                Die();
            else
                _tankHit.Show();
        }

        private void Die()
        {
            _tankDeath.Show();
            _tankMovement.Off();
            _objectsMover.Stop();
            _gameOverWindow.Open();
        }
    }
}