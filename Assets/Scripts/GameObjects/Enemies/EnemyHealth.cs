using System;
using Pool.Enemies;
using UnityEngine;

namespace GameObjects.Enemies
{
    public class EnemyHealth : MonoBehaviour, IHealth
    {
        private float _armor;
        private EnemyDeathVfx _enemyDeathVfx;
        private IEnemiesGenerator _enemiesGenerator;

        public float Max { get; private set; }
        public float Current { get; private set; }

        public event Action HealthChanged;

        public void Construct(float health, float armor, EnemyDeathVfx enemyDeathVfx,
            IEnemiesGenerator enemiesGenerator)
        {
            _enemiesGenerator = enemiesGenerator;
            _enemyDeathVfx = enemyDeathVfx;
            _armor = armor;
            Max = health;
            Current = health;
        }

        public void GetHit(float damage)
        {
            Current -= damage * _armor;

            if (Current <= 0f)
            {
                _enemyDeathVfx.Show();
                _enemiesGenerator.ReduceActiveEnemies();
                gameObject.SetActive(false);
            }

            HealthChanged?.Invoke();
        }
    }
}