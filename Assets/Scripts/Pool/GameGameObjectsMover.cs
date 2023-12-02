using System.Collections.Generic;
using GameObjects;
using GameObjects.Enemies;
using Pool.Enemies;
using UnityEngine;

namespace Pool
{
    public class GameGameObjectsMover : MonoBehaviour, IGameObjectsMover
    {
        private IEnemiesGenerator _enemiesGenerator;

        public void Construct(IEnemiesGenerator enemiesGenerator)
        {
            _enemiesGenerator = enemiesGenerator;
        }

        public void Run()
        {
            if (_enemiesGenerator == null)
                return;

            if (_enemiesGenerator.ProjectilesDictionary == null)
                return;

            if (_enemiesGenerator.ProjectilesDictionary.Count == 0)
                return;

            foreach (KeyValuePair<EnemyTypeId, List<GameObject>> pair in _enemiesGenerator.ProjectilesDictionary)
            {
                foreach (GameObject gameObject in pair.Value)
                    gameObject.GetComponent<IGameObjectMovement>()?.Run();
            }
        }

        public void Stop()
        {
            if (_enemiesGenerator == null)
                return;

            if (_enemiesGenerator.ProjectilesDictionary == null)
                return;

            if (_enemiesGenerator.ProjectilesDictionary.Count == 0)
                return;

            foreach (KeyValuePair<EnemyTypeId, List<GameObject>> pair in _enemiesGenerator.ProjectilesDictionary)
            {
                foreach (GameObject gameObject in pair.Value)
                    gameObject.GetComponent<IGameObjectMovement>()?.Stop();
            }
        }
    }
}