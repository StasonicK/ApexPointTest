using System.Collections.Generic;
using GameObjects;
using GameObjects.Enemies;
using UnityEngine;

namespace Pool
{
    public class GameGameObjectsMover : MonoBehaviour, IGameObjectsMover
    {
        private IGameObjectsGenerator _gameObjectsGenerator;

        public void Construct(IGameObjectsGenerator gameObjectsGenerator)
        {
            _gameObjectsGenerator = gameObjectsGenerator;
        }

        public void Run()
        {
            if (_gameObjectsGenerator == null)
                return;

            if (_gameObjectsGenerator.GameObjectsDictionary == null)
                return;

            if (_gameObjectsGenerator.GameObjectsDictionary.Count == 0)
                return;

            foreach (KeyValuePair<EnemyType, List<GameObject>> pair in _gameObjectsGenerator.GameObjectsDictionary)
            {
                foreach (GameObject gameObject in pair.Value)
                    gameObject.GetComponent<IGameObjectMovement>()?.Run();
            }
        }

        public void Stop()
        {
            if (_gameObjectsGenerator == null)
                return;

            if (_gameObjectsGenerator.GameObjectsDictionary == null)
                return;

            if (_gameObjectsGenerator.GameObjectsDictionary.Count == 0)
                return;

            foreach (KeyValuePair<EnemyType, List<GameObject>> pair in _gameObjectsGenerator.GameObjectsDictionary)
            {
                foreach (GameObject gameObject in pair.Value)
                    gameObject.GetComponent<IGameObjectMovement>()?.Stop();
            }
        }
    }
}