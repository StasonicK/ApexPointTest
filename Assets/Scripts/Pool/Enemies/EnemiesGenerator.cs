using System.Collections.Generic;
using System.Linq;
using GameObjects;
using GameObjects.Enemies;
using Services.StaticData;
using StaticData;
using UnityEngine;

namespace Pool.Enemies
{
    public class EnemiesGenerator : MonoBehaviour, IEnemiesGenerator
    {
        private const string EnemySpawnPointTag = "EnemySpawnPoint";
        [SerializeField] private Enemy1Type _enemy1Prefab;
        [SerializeField] private Enemy2Type _enemy2Prefab;

        private float _yAddition = 1.0f;
        private IContainer _container;
        private float _secondsBetweenSpawns;
        private float _elapsedTime;
        private int _currentIndexGameObjectType = Constants.InitialIndex;
        private int _previousIndexGameObjectType = Constants.InitialIndex;
        private int _currentIndexSpawnPosition = Constants.InitialIndex;
        private int _previousIndexSpawnPosition = Constants.InitialIndex;
        private GameObject _enemy;
        private List<GameObject> _gameObjectsList;
        private EnemyTypeId _enemyTypeId;
        private bool _isEnabled;
        private List<Vector3> _spawnPositions;
        private List<Vector3> _invisibleSpawnPositions;
        private Transform _tankTransform;
        private Vector3 _randomSpawnPosition;
        private IStaticDataService _staticDataService;
        private EnemyStaticData _enemyStaticData;
        private EnemyMovement _enemyMovement;
        private EnemyHealth _enemyHealth;
        private EnemyDeathVfx _enemyDeathVfx;
        private EnemyCollisionHandler _enemyCollisionHandler;
        private int _activeEnemies;
        private int _maxActiveCount;
        private Camera _cameraMain;
        private GameObject[] _enemySpawnPoints;

        public GameObject GameObject => gameObject;
        public Dictionary<EnemyTypeId, List<GameObject>> ProjectilesDictionary { get; private set; }

        private void Awake() =>
            DontDestroyOnLoad(this);

        private void OnEnable() =>
            _cameraMain = Camera.main;

        private void Update()
        {
            if (!_isEnabled)
                return;

            if (_activeEnemies >= _maxActiveCount)
                return;

            _elapsedTime += Time.deltaTime;

            if (_elapsedTime > _secondsBetweenSpawns)
            {
                _elapsedTime = 0;
                Generate();
            }
        }

        public void Construct(IStaticDataService staticDataService, IContainer container,
            int enemies1Count, int enemies2Count, float secondsBetweenSpawns, List<Vector3> spawnPositions,
            Transform tankTransform, int maxActiveCount)
        {
            _maxActiveCount = maxActiveCount;
            _staticDataService = staticDataService;
            _tankTransform = tankTransform;
            _container = container;
            _spawnPositions = spawnPositions;
            _secondsBetweenSpawns = secondsBetweenSpawns;
            ProjectilesDictionary = new Dictionary<EnemyTypeId, List<GameObject>>();

            DestroyAll();
            CreateEnemies(enemies1Count, _enemy1Prefab, EnemyTypeId.Enemy1);
            CreateEnemies(enemies2Count, _enemy2Prefab, EnemyTypeId.Enemy2);
            _elapsedTime = _secondsBetweenSpawns;
            _invisibleSpawnPositions = new List<Vector3>();
        }

        public void ReduceActiveEnemies() =>
            _activeEnemies--;

        private void Generate()
        {
            if (TryGetEnemy(GetEnemyTypeId(), out GameObject enemy))
            {
                enemy.transform.position = GetSpawnPoint();
                enemy.SetActive(true);
                _activeEnemies++;
            }
        }

        private void DestroyAll()
        {
            if (ProjectilesDictionary == null)
                return;

            foreach (KeyValuePair<EnemyTypeId, List<GameObject>> pair in ProjectilesDictionary)
            {
                if (pair.Value != null)
                    for (int i = 0; i < pair.Value.Count; i++)
                        Destroy(pair.Value[i]);
            }

            _container.Clear();
        }

        private void CreateEnemies(int enemiesCount, BaseEnemyType prefab, EnemyTypeId typeId)
        {
            if (enemiesCount == 0)
                return;

            List<GameObject> enemiesList = new List<GameObject>(enemiesCount);

            for (int i = 0; i < enemiesCount; i++)
            {
                _enemy = Instantiate(prefab.gameObject, _container.GameObject.transform);
                ConstructEnemy(typeId);
                _enemy.SetActive(false);
                _enemy.GetComponent<IGameObjectMovement>().Run();
                enemiesList.Add(_enemy);
                _container.Add(_enemy);
            }

            ProjectilesDictionary[typeId] = enemiesList;
            _gameObjectsList = null;
        }

        private void ConstructEnemy(EnemyTypeId enemyTypeId)
        {
            _enemyStaticData = _staticDataService.ForEnemy(enemyTypeId);
            _enemyMovement = _enemy.GetComponent<EnemyMovement>();
            _enemyHealth = _enemy.GetComponent<EnemyHealth>();
            _enemyDeathVfx = _enemy.GetComponent<EnemyDeathVfx>();
            _enemyCollisionHandler = _enemy.GetComponent<EnemyCollisionHandler>();
            _enemyMovement.Construct(_tankTransform, _enemyStaticData.Speed);
            _enemyHealth.Construct(_enemyStaticData.Health, _enemyStaticData.Armor, _enemyDeathVfx, this);
            _enemyCollisionHandler.Construct(_enemyStaticData.Damage);
        }

        private bool TryGetEnemy(EnemyTypeId typeId, out GameObject result)
        {
            result = ProjectilesDictionary[typeId].FirstOrDefault(p => p.activeSelf == false);
            return result != null;
        }

        private EnemyTypeId GetEnemyTypeId()
        {
            while (_currentIndexGameObjectType == _previousIndexGameObjectType)
                _currentIndexGameObjectType = Random.Range(0, ProjectilesDictionary.Count);

            _previousIndexGameObjectType = _currentIndexGameObjectType;

            for (int i = 0; i < ProjectilesDictionary.Count(); i++)
            {
                if (i == _currentIndexGameObjectType)
                {
                    _enemyTypeId = ProjectilesDictionary.Keys.ElementAt(i);
                    break;
                }
            }

            return _enemyTypeId;
        }

        private Vector3 GetSpawnPoint()
        {
            OutputVisibleRenderers();

            while (_currentIndexSpawnPosition == _previousIndexSpawnPosition)
                _currentIndexSpawnPosition = Random.Range(0, _invisibleSpawnPositions.Count);

            _previousIndexSpawnPosition = _currentIndexSpawnPosition;
            _randomSpawnPosition = _invisibleSpawnPositions[_currentIndexSpawnPosition];
            Vector3 spawnPosition = _randomSpawnPosition.AddY(_yAddition);
            Debug.Log($"GetSpawnPoint _currentIndexSpawnPosition {_currentIndexSpawnPosition}");
            Debug.Log($"GetSpawnPoint spawnPosition {spawnPosition}");
            return spawnPosition;
        }

        private void OutputVisibleRenderers()
        {
            if (_invisibleSpawnPositions != null)
                _invisibleSpawnPositions.Clear();

            _invisibleSpawnPositions = new List<Vector3>();

            foreach (Vector3 spawnPosition in _spawnPositions)
            {
                if (!IsInView(spawnPosition))
                    _invisibleSpawnPositions.Add(spawnPosition);
            }

            Debug.Log($"_invisibleSpawnPositions {_invisibleSpawnPositions.Count}");
        }

        private bool IsInView(Vector3 worldPos)
        {
            Transform camTransform = _cameraMain.transform;
            Vector2 viewPos = _cameraMain.WorldToViewportPoint(worldPos);
            Vector3 dir = (worldPos - camTransform.position).normalized;
            float dot = Vector3.Dot(camTransform.forward, dir);

            Debug.Log($"dot {dot}");

            if (dot > 0 && viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1)
                return true;
            else
                return false;
        }

        public void Reset()
        {
            foreach (KeyValuePair<EnemyTypeId, List<GameObject>> pair in ProjectilesDictionary)
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