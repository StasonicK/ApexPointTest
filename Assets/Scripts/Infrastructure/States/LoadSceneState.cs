using System;
using Audio;
using GameObjects.Tank;
using Pool;
using Services.PersistentProgress;
using Services.SaveLoad;
using Services.StaticData;
using StaticData;
using UI.Screens.GameLoop;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Infrastructure.States
{
    public class LoadSceneState : IPayloadedState<SceneId>
    {
        private const string LevelName = "Level_";

        private readonly IGameStateMachine _gameStateMachine;
        private readonly ISceneLoader _sceneLoader;
        private readonly IStaticDataService _staticDataService;
        private readonly ISaveLoadService _saveLoadService;
        private readonly IPlayerProgressService _playerProgressService;
        private SceneId _sceneId;
        private bool _isInitial = true;
        private GameObject _tank;
        private IObjectsContainer _objectsContainer;
        private IObjectsGenerator _objectsGenerator;
        private IUIContainer _uiContainer;
        private LevelStaticData _levelStaticData;
        private Vector3 _tankSpawnPosition;
        private TankMovement _tankMovement;
        private IObjectsMover _objectsMover;
        private TankRotation _tankRotation;
        private TankShooting _tankShooting;

        public LoadSceneState(IGameStateMachine gameGameStateMachine, ISceneLoader sceneLoader,
            IPlayerProgressService playerProgressService, ISaveLoadService saveLoadService,
            IStaticDataService staticDataService, GameObject tank, IUIContainer uiContainer,
            IObjectsContainer objectsContainer, IObjectsGenerator objectsGenerator, IObjectsMover objectsMover)
        {
            _objectsGenerator = objectsGenerator;
            _objectsMover = objectsMover;
            _gameStateMachine = gameGameStateMachine;
            _sceneLoader = sceneLoader;
            _playerProgressService = playerProgressService;
            _saveLoadService = saveLoadService;
            _staticDataService = staticDataService;
            _tank = tank;
            _uiContainer = uiContainer;
            _objectsContainer = objectsContainer;
            _tankMovement = _tank.GetComponent<TankMovement>();
            _tankRotation = _tank.GetComponentInChildren<TankRotation>();
            _tankShooting = _tank.GetComponentInChildren<TankShooting>();
        }

        public void Enter(SceneId sceneId)
        {
            _sceneId = sceneId;

            if (_sceneId.ToString() == SceneId.Initial.ToString())
                return;

            if (_sceneId.ToString().Contains(LevelName))
                _sceneLoader.Load(_sceneId, InitializeGameWorld);
            else
                _sceneLoader.Load(_sceneId, ClearGameWorld);

            LaunchAudioManager();
            _gameStateMachine.Enter<GameLoopState>();
        }

        public void Exit()
        {
        }

        private void ClearGameWorld()
        {
            _tank.SetActive(false);
            _objectsContainer.GameObject.SetActive(false);
            _objectsGenerator.GameObject.SetActive(false);
            _uiContainer.HideAll();
        }

        private void InitializeGameWorld()
        {
            _levelStaticData = LevelStaticData();
            InitializeUIRoot(_levelStaticData.KilledEnemiesVictoryCount);

            if (_levelStaticData.GameLoopLevel)
            {
                InitializeGameWorld(_levelStaticData);
                InstantiateGameObjects();
                InitializeGameObjects();
                // _uiContainer.ShowStartGameWindow();
            }
        }

        private void InitializeUIRoot(int victoryCount)
        {
            _uiContainer.GameObject.SetActive(true);
            _uiContainer.StartGameWindow.Construct(_objectsMover, _objectsGenerator, _uiContainer.GameScreen,
                _tankMovement, _tankRotation, _tankShooting);
            _uiContainer.GameOverWindow.Construct(_objectsGenerator, _tankMovement, _tankRotation, _tankShooting,
                _gameStateMachine);
            _uiContainer.FinishLevelWindow.Construct(_sceneId, victoryCount);
            _uiContainer.Construct();
            _uiContainer.GameScreen.GetComponentInChildren<MoneyCounter>().Construct();
            _uiContainer.PrepareLevel();
        }

        private LevelStaticData LevelStaticData()
        {
            SceneId sceneId = Enum.Parse<SceneId>(SceneManager.GetActiveScene().name);
            return _staticDataService.ForLevel(sceneId);
        }

        private void InitializeGameWorld(LevelStaticData levelData)
        {
            _tankSpawnPosition = levelData.TankSpawnPosition;
            _objectsGenerator.Construct(levelData.Enemy1Count, levelData.Enemy2Count,
                levelData.SecondsBetweenSpawns, _objectsContainer);
            _objectsMover.Construct(_objectsGenerator);
            _objectsContainer.GameObject.SetActive(true);
        }

        private void InstantiateGameObjects()
        {
            _tank.transform.position = _tankSpawnPosition;
            _tank.SetActive(true);
        }

        private void InitializeGameObjects()
        {
            _tankMovement.Construct(_levelStaticData.TankSpawnPosition);
            _tankRotation.Construct();
            _tank.GetComponent<TankHealth>().Construct(_tankMovement, _tank.GetComponent<TankDeath>(),
                _tank.GetComponent<TankHit>(), _objectsMover, _uiContainer.GameOverWindow);
        }

        private void LaunchAudioManager()
        {
            if (_playerProgressService.SettingsData.IsAudioOn)
            {
                AudioManager.Instance.Unmute();
                AudioManager.Instance.ChangeVolume(Constants.MaxValue);
                // AudioManager.Instance.ChangeVolume(_playerProgressService.SettingsData.Volume);
            }
            else
            {
                AudioManager.Instance.Mute();
            }
        }
    }
}