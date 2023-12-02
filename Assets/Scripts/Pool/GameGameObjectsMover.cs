using System.Collections.Generic;
using GameObjects;
using GameObjects.Enemies;
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

            if (_enemiesGenerator.EnemiesDictionary == null)
                return;

            if (_enemiesGenerator.EnemiesDictionary.Count == 0)
                return;

            foreach (KeyValuePair<EnemyTypeId, List<GameObject>> pair in _enemiesGenerator.EnemiesDictionary)
            {
                foreach (GameObject gameObject in pair.Value)
                    gameObject.GetComponent<IGameObjectMovement>()?.Run();
            }
        }

        public void Stop()
        {
            if (_enemiesGenerator == null)
                return;

            if (_enemiesGenerator.EnemiesDictionary == null)
                return;

            if (_enemiesGenerator.EnemiesDictionary.Count == 0)
                return;

            foreach (KeyValuePair<EnemyTypeId, List<GameObject>> pair in _enemiesGenerator.EnemiesDictionary)
            {
                foreach (GameObject gameObject in pair.Value)
                    gameObject.GetComponent<IGameObjectMovement>()?.Stop();
            }
        }
    }
}