using System.Collections.Generic;
using GameObjects.Tank;
using Pool.Enemies;
using Services.StaticData;
using UnityEngine;

namespace Pool.Projectiles
{
    public interface IProjectilesGenerator : IGameObject, IGenerator
    {
        Dictionary<WeaponTypeId, List<GameObject>> ProjectilesDictionary { get; }

        void Construct(IStaticDataService staticDataService, IProjectilesContainer projectilesContainer,
            IEnemiesGenerator enemiesGenerator, int bigGunProjectilesCount, int machineGunProjectilesCount);

        void GetProjectile(WeaponTypeId weaponTypeId, Transform transform);
    }
}