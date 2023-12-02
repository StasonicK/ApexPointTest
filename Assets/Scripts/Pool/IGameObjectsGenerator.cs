using System.Collections.Generic;
using GameObjects.Enemies;
using UnityEngine;

namespace Pool
{
    public interface IGameObjectsGenerator : IGameObject
    {
        Dictionary<EnemyType, List<GameObject>> GameObjectsDictionary { get; }

        void Construct(int enemies1Count, int enemies2Count, float secondsBetweenSpawns,
            IGameObjectsContainer gameObjectsContainer);

        bool TryGetObject(EnemyType type, out GameObject result);
        void Reset();
        void On();
        void Off();
    }
}