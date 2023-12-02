using System.Collections.Generic;
using GameObjects.Enemies;
using UnityEngine;

namespace Pool
{
    public interface IEnemiesGenerator : IGameObject
    {
        Dictionary<EnemyTypeId, List<GameObject>> EnemiesDictionary { get; }

        void Construct(IEnemiesContainer enemiesContainer, int enemies1Count, int enemies2Count,
            float secondsBetweenSpawns, List<Vector3> spawnPositions, Transform tankTransform);

        // bool TryGetEnemy(EnemyTypeId typeId, out GameObject result);
        void Reset();
        void On();
        void Off();
    }
}