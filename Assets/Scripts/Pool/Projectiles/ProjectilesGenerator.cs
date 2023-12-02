using System.Collections.Generic;
using System.Linq;
using GameObjects;
using GameObjects.Projectiles;
using GameObjects.Tank;
using Pool.Enemies;
using Services.StaticData;
using StaticData;
using UnityEngine;

namespace Pool.Projectiles
{
    public class ProjectilesGenerator : MonoBehaviour, IProjectilesGenerator
    {
        [SerializeField] private BigGunType _bigGunTypePrefab;
        [SerializeField] private MachineGunType _machineGunTypePrefab;

        private IContainer _container;
        private float _secondsBetweenSpawns;
        private int _currentIndexGameObjectType = Constants.InitialIndex;
        private int _previousIndexGameObjectType = Constants.InitialIndex;
        private int _currentIndexSpawnPosition = Constants.InitialIndex;
        private int _previousIndexSpawnPosition = Constants.InitialIndex;
        private GameObject _projectile;
        private List<GameObject> _gameObjectsList;
        private WeaponTypeId _weaponTypeId;
        private bool _isEnabled;
        private Transform _tankTransform;
        private Vector3 _randomSpawnPosition;
        private IStaticDataService _staticDataService;
        private WeaponStaticData _weaponStaticData;
        private IEnemiesGenerator _enemiesGenerator;

        public GameObject GameObject => gameObject;
        public Dictionary<WeaponTypeId, List<GameObject>> ProjectilesDictionary { get; private set; }

        private void Awake() =>
            DontDestroyOnLoad(this);

        public void Construct(IStaticDataService staticDataService, IProjectilesContainer projectilesContainer,
            IEnemiesGenerator enemiesGenerator, int bigGunProjectilesCount, int machineGunProjectilesCount)
        {
            _enemiesGenerator = enemiesGenerator;
            _staticDataService = staticDataService;
            _container = projectilesContainer;
            ProjectilesDictionary = new Dictionary<WeaponTypeId, List<GameObject>>();

            DestroyAll();
            CreateProjectiles(bigGunProjectilesCount, _bigGunTypePrefab, WeaponTypeId.BigGun);
            CreateProjectiles(machineGunProjectilesCount, _machineGunTypePrefab, WeaponTypeId.MachineGun);
        }

        public void GetProjectile(WeaponTypeId weaponTypeId, Transform transform)
        {
            if (TryGetProjectile(weaponTypeId, out GameObject projectile))
            {
                projectile.transform.position = transform.position;
                projectile.transform.rotation = transform.rotation;
                projectile.SetActive(true);
            }
        }

        private void DestroyAll()
        {
            if (ProjectilesDictionary == null)
                return;

            foreach (KeyValuePair<WeaponTypeId, List<GameObject>> pair in ProjectilesDictionary)
            {
                if (pair.Value != null)
                    for (int i = 0; i < pair.Value.Count; i++)
                        Destroy(pair.Value[i]);
            }

            _container.Clear();
        }

        private void CreateProjectiles(int projectilesCount, BaseGunType prefab, WeaponTypeId weaponTypeId)
        {
            if (projectilesCount == 0)
                return;

            List<GameObject> projectilesList = new List<GameObject>(projectilesCount);

            for (int i = 0; i < projectilesCount; i++)
            {
                _projectile = Instantiate(prefab.gameObject, _container.GameObject.transform);
                ConstructProjectile(weaponTypeId);
                _projectile.SetActive(false);
                _projectile.GetComponent<IGameObjectMovement>().Run();
                projectilesList.Add(_projectile);
                _container.Add(_projectile);
            }

            ProjectilesDictionary[weaponTypeId] = projectilesList;
            _gameObjectsList = null;
        }

        private void ConstructProjectile(WeaponTypeId weaponTypeId)
        {
            _weaponStaticData = _staticDataService.ForWeapon(weaponTypeId);
            _projectile.GetComponentInChildren<ProjectileCollisionHandler>()
                .Construct(_weaponStaticData.Damage, _enemiesGenerator);
        }

        private bool TryGetProjectile(WeaponTypeId weaponTypeId, out GameObject result)
        {
            result = ProjectilesDictionary[weaponTypeId].FirstOrDefault(p => p.activeSelf == false);
            return result != null;
        }


        public void Reset()
        {
            foreach (KeyValuePair<WeaponTypeId, List<GameObject>> pair in ProjectilesDictionary)
            {
                if (pair.Value.Count > 0)
                    foreach (GameObject item in pair.Value)
                    {
                        item.GetComponent<IGameObjectMovement>()?.Run();
                        item.SetActive(false);
                    }
            }
        }

        public void On() =>
            _isEnabled = true;

        public void Off() =>
            _isEnabled = false;
    }
}