using GameObjects.Enemies;
using GameObjects.Tank;
using Infrastructure;
using StaticData;

namespace Services.StaticData
{
    public interface IStaticDataService : IService
    {
        void Load();

        LevelStaticData ForLevel(SceneId sceneId);
        EnemyStaticData ForEnemy(EnemyTypeId enemyTypeId);
        WeaponStaticData ForWeapon(WeaponTypeId weaponTypeId);
    }
}