using System.Collections.Generic;
using System.Linq;
using GameObjects;
using GameObjects.Enemy1;
using GameObjects.Enemy2;
using StaticData;
using UnityEngine;

namespace Pool
{
    public class ObjectsGenerator : MonoBehaviour, IObjectsGenerator
    {
        [SerializeField] private Enemy1Type _enemy1Prefab;
        [SerializeField] private Enemy2Type _enemy2Prefab;
        [SerializeField] private float _ySpawnPosition;

        private float _secondsBetweenSpawns;
        private float _elapsedTime;
        private IObjectsContainer _objectsContainer;
        private int _currentIndexGameObjectType = Constants.InitialIndex;
        private int _currentIndexPositionTypeindex = Constants.InitialIndex;
        private int _previousIndexGameObjectType = Constants.InitialIndex;
        private int _previousIndexPositionType = Constants.InitialIndex;
        private GameObject _gameObject;
        private List<GameObject> _gameObjectsList;
        private List<PositionType> _positionTypes = Extensions.GetValues<PositionType>().ToList();
        private PositionType _positionType;
        private GameObjectType _gameObjectType;
        private bool _isEnabled;

        public Dictionary<GameObjectType, List<GameObject>> GameObjectsDictionary { get; private set; }

        public GameObject GameObject => gameObject;

        private void Awake() =>
            DontDestroyOnLoad(this);

        // private void OnEnable() =>
        //     _isGameLoop = false;

        private void Update()
        {
            if (!_isEnabled)
                return;

            _elapsedTime += Time.deltaTime;

            if (_elapsedTime > _secondsBetweenSpawns)
            {
                _elapsedTime = 0;

                // if (_isGameLoop)
                // GenerateGameLoop();
                // else
                //     GenerateForIdle();
            }
        }

        public void Construct(int enemies1Count, int enemies2Count, float secondsBetweenSpawns,
            IObjectsContainer objectsContainer)
        {
            _secondsBetweenSpawns = secondsBetweenSpawns;
            GameObjectsDictionary = new Dictionary<GameObjectType, List<GameObject>>();
            _objectsContainer = objectsContainer;

            DestroyAll();
            // CreateEnemies(enemies1Count, _enemy1Prefab, GameObjectType.Enemy1);
            // CreateEnemies(enemies2Count, _enemy2Prefab, GameObjectType.Enemy2);
            _elapsedTime = _secondsBetweenSpawns;
        }

        private void GenerateGameLoop()
        {
            if (TryGetObject(GetGameObjectType(), out GameObject obstacle))
            {
                obstacle.transform.position = GetSpawnPoint();
                obstacle.SetActive(true);
            }
        }

        private void DestroyAll()
        {
            if (GameObjectsDictionary == null)
                return;

            foreach (KeyValuePair<GameObjectType, List<GameObject>> pair in GameObjectsDictionary)
            {
                if (pair.Value != null)
                    for (int i = 0; i < pair.Value.Count; i++)
                        Destroy(pair.Value[i]);
            }

            _objectsContainer.Clear();
        }

        private void CreateEnemies(int enemiesCount, EnemyType prefab, GameObjectType type)
        {
            if (enemiesCount == 0)
                return;

            List<GameObject> gameObjectsList = new List<GameObject>(enemiesCount);

            for (int i = 0; i < enemiesCount; i++)
            {
                _gameObject = Instantiate(prefab.gameObject, _objectsContainer.GameObject.transform);
                _gameObject.SetActive(false);
                // _gameObject.GetComponent<GameObjectMovement>().Run();
                gameObjectsList.Add(_gameObject);
                _objectsContainer.Add(_gameObject);
            }

            GameObjectsDictionary[type] = gameObjectsList;
            _gameObjectsList = null;
        }

        public bool TryGetObject(GameObjectType type, out GameObject result)
        {
            result = GameObjectsDictionary[type].FirstOrDefault(p => p.activeSelf == false);
            return result != null;
        }

        private GameObjectType GetGameObjectType()
        {
            while (_currentIndexGameObjectType == _previousIndexGameObjectType)
                _currentIndexGameObjectType = Random.Range(0, GameObjectsDictionary.Count);

            _previousIndexGameObjectType = _currentIndexGameObjectType;

            for (int i = 0; i < GameObjectsDictionary.Count(); i++)
            {
                if (i == _currentIndexGameObjectType)
                {
                    _gameObjectType = GameObjectsDictionary.Keys.ElementAt(i);
                    break;
                }
            }

            return _gameObjectType;
        }

        private Vector3 GetSpawnPoint()
        {
            // TODO change logic and add to LateUpdate() may be

            while (_currentIndexPositionTypeindex == _previousIndexPositionType)
                _currentIndexPositionTypeindex = Random.Range(0, _positionTypes.Count);

            _previousIndexPositionType = _currentIndexPositionTypeindex;
            return new Vector3();
            // switch (_positionTypes[_currentIndexPositionTypeindex])
            // {
            //     case PositionType.Left:
            //         return new Vector3(_xLeftSpawnPosition, _ySpawnPosition, Constants.GameObjectsZPosition);
            //     case PositionType.Right:
            //         return new Vector3(_xRightSpawnPosition, _ySpawnPosition, Constants.GameObjectsZPosition);
            //     default:
            //         return new Vector3(_xCenterSpawnPosition, _ySpawnPosition, Constants.GameObjectsZPosition);
            // }
        }

        public void Reset()
        {
            foreach (KeyValuePair<GameObjectType, List<GameObject>> pair in GameObjectsDictionary)
            {
                if (pair.Value.Count > 0)
                    foreach (GameObject item in pair.Value)
                    {
                        item.GetComponent<GameObjectMovement>()?.Run();
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