using System.Collections.Generic;
using System.Linq;
using GameObjects;
using GameObjects.Enemies;
using UnityEngine;

namespace Pool
{
    public class EnemiesGenerator : MonoBehaviour, IEnemiesGenerator
    {
        [SerializeField] private Enemy1Type _enemy1Prefab;
        [SerializeField] private Enemy2Type _enemy2Prefab;

        private float _yAddition = 1.0f;
        private IEnemiesContainer _enemiesContainer;
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
        private Transform _tankTransform;
        private Vector3 _randomSpawnPosition;

        public GameObject GameObject => gameObject;
        public Dictionary<EnemyTypeId, List<GameObject>> EnemiesDictionary { get; private set; }

        private void Awake() =>
            DontDestroyOnLoad(this);

        private void Update()
        {
            if (!_isEnabled)
                return;

            _elapsedTime += Time.deltaTime;

            if (_elapsedTime > _secondsBetweenSpawns)
            {
                _elapsedTime = 0;
                Generate();
            }
        }

        public void Construct(IEnemiesContainer enemiesContainer, int enemies1Count, int enemies2Count,
            float secondsBetweenSpawns, List<Vector3> spawnPositions, Transform tankTransform)
        {
            _tankTransform = tankTransform;
            _enemiesContainer = enemiesContainer;
            _spawnPositions = spawnPositions;
            _secondsBetweenSpawns = secondsBetweenSpawns;
            EnemiesDictionary = new Dictionary<EnemyTypeId, List<GameObject>>();

            DestroyAll();
            CreateEnemies(enemies1Count, _enemy1Prefab, EnemyTypeId.Enemy1);
            CreateEnemies(enemies2Count, _enemy2Prefab, EnemyTypeId.Enemy2);
            _elapsedTime = _secondsBetweenSpawns;
        }

        private void Generate()
        {
            if (TryGetEnemy(GetEnemyTypeId(), out GameObject enemy))
            {
                enemy.transform.position = GetSpawnPoint();
                enemy.SetActive(true);
            }
        }

        private void DestroyAll()
        {
            if (EnemiesDictionary == null)
                return;

            foreach (KeyValuePair<EnemyTypeId, List<GameObject>> pair in EnemiesDictionary)
            {
                if (pair.Value != null)
                    for (int i = 0; i < pair.Value.Count; i++)
                        Destroy(pair.Value[i]);
            }

            _enemiesContainer.Clear();
        }

        private void CreateEnemies(int enemiesCount, BaseEnemyType prefab, EnemyTypeId typeId)
        {
            if (enemiesCount == 0)
                return;

            List<GameObject> enemiesList = new List<GameObject>(enemiesCount);

            for (int i = 0; i < enemiesCount; i++)
            {
                _enemy = Instantiate(prefab.gameObject, _enemiesContainer.GameObject.transform);

                if (typeId == EnemyTypeId.Enemy1)
                    _enemy.GetComponent<EnemyMoveToHero>().Construct(_tankTransform, 3f);
                else
                    _enemy.GetComponent<EnemyMoveToHero>().Construct(_tankTransform, 2f);

                _enemy.SetActive(false);
                _enemy.GetComponent<IGameObjectMovement>().Run();
                enemiesList.Add(_enemy);
                _enemiesContainer.Add(_enemy);
            }

            EnemiesDictionary[typeId] = enemiesList;
            _gameObjectsList = null;
        }

        private bool TryGetEnemy(EnemyTypeId typeId, out GameObject result)
        {
            result = EnemiesDictionary[typeId].FirstOrDefault(p => p.activeSelf == false);
            Debug.Log($"TryGetObject result {result}");
            return result != null;
        }

        private EnemyTypeId GetEnemyTypeId()
        {
            while (_currentIndexGameObjectType == _previousIndexGameObjectType)
                _currentIndexGameObjectType = Random.Range(0, EnemiesDictionary.Count);

            _previousIndexGameObjectType = _currentIndexGameObjectType;

            for (int i = 0; i < EnemiesDictionary.Count(); i++)
            {
                if (i == _currentIndexGameObjectType)
                {
                    _enemyTypeId = EnemiesDictionary.Keys.ElementAt(i);
                    break;
                }
            }

            Debug.Log($"GetEnemyTypeId enemyTypeId {_enemyTypeId}");
            return _enemyTypeId;
        }

        private Vector3 GetSpawnPoint()
        {
            while (_currentIndexSpawnPosition == _previousIndexSpawnPosition)
                _currentIndexSpawnPosition = Random.Range(0, _spawnPositions.Count);

            _previousIndexSpawnPosition = _currentIndexSpawnPosition;
            _randomSpawnPosition = _spawnPositions[_currentIndexSpawnPosition];
            Vector3 spawnPosition = _randomSpawnPosition.AddY(_yAddition);
            Debug.Log($"GetSpawnPoint _currentIndexSpawnPosition {_currentIndexSpawnPosition}");
            Debug.Log($"GetSpawnPoint spawnPosition {spawnPosition}");
            return spawnPosition;
        }

        public void Reset()
        {
            foreach (KeyValuePair<EnemyTypeId, List<GameObject>> pair in EnemiesDictionary)
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