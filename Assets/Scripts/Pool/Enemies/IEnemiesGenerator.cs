using System.Collections.Generic;
using GameObjects.Enemies;
using Services.StaticData;
using UnityEngine;

namespace Pool.Enemies
{
    public interface IEnemiesGenerator : IGameObject, IGenerator
    {
        Dictionary<EnemyTypeId, List<GameObject>> ProjectilesDictionary { get; }

        void Construct(IStaticDataService staticDataService, IContainer container, int enemies1Count, int enemies2Count,
            float secondsBetweenSpawns, List<Vector3> spawnPositions, Transform tankTransform, int maxActiveCount);

        void ReduceActiveEnemies();
    }
}