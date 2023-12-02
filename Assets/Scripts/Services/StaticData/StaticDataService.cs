using System.Collections.Generic;
using System.Linq;
using GameObjects.Enemies;
using GameObjects.Tank;
using Infrastructure;
using StaticData;
using UnityEngine;

namespace Services.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private const string LevelsPath = "StaticData/Levels";
        private const string EnemiesPath = "StaticData/Enemies";
        private const string WeaponsPath = "StaticData/Weapons";

        private Dictionary<SceneId, LevelStaticData> _levels;
        private Dictionary<EnemyTypeId, EnemyStaticData> _enemies;
        private Dictionary<WeaponTypeId, WeaponStaticData> _weapons;

        public void Load()
        {
            _levels = Resources
                .LoadAll<LevelStaticData>(LevelsPath)
                .ToDictionary(x => x.SceneId, x => x);

            _enemies = Resources
                .LoadAll<EnemyStaticData>(EnemiesPath)
                .ToDictionary(x => x.EnemyTypeId, x => x);

            _weapons = Resources
                .LoadAll<WeaponStaticData>(WeaponsPath)
                .ToDictionary(x => x.WeaponTypeId, x => x);
        }

        public LevelStaticData ForLevel(SceneId sceneId) =>
            _levels.TryGetValue(sceneId, out LevelStaticData staticData)
                ? staticData
                : null;

        public EnemyStaticData ForEnemy(EnemyTypeId enemyTypeId) =>
            _enemies.TryGetValue(enemyTypeId, out EnemyStaticData staticData)
                ? staticData
                : null;

        public WeaponStaticData ForWeapon(WeaponTypeId weaponTypeId) =>
            _weapons.TryGetValue(weaponTypeId, out WeaponStaticData staticData)
                ? staticData
                : null;
    }
}