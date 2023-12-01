using System.Collections.Generic;
using GameObjects;
using UnityEngine;

namespace Pool
{
    public class ObjectsMover : MonoBehaviour, IObjectsMover
    {
        private IObjectsGenerator _objectsGenerator;

        public void Construct(IObjectsGenerator objectsGenerator)
        {
            _objectsGenerator = objectsGenerator;
        }

        public void Run()
        {
            if (_objectsGenerator == null)
                return;

            if (_objectsGenerator.GameObjectsDictionary == null)
                return;

            if (_objectsGenerator.GameObjectsDictionary.Count == 0)
                return;

            foreach (KeyValuePair<GameObjectType, List<GameObject>> pair in _objectsGenerator.GameObjectsDictionary)
            {
                foreach (GameObject gameObject in pair.Value)
                    gameObject.GetComponent<GameObjectMovement>()?.Run();
            }
        }

        public void Stop()
        {
            if (_objectsGenerator == null)
                return;

            if (_objectsGenerator.GameObjectsDictionary == null)
                return;

            if (_objectsGenerator.GameObjectsDictionary.Count == 0)
                return;

            foreach (KeyValuePair<GameObjectType, List<GameObject>> pair in _objectsGenerator.GameObjectsDictionary)
            {
                foreach (GameObject gameObject in pair.Value)
                    gameObject.GetComponent<GameObjectMovement>()?.Stop();
            }
        }
    }
}