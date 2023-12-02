using System;
using Audio;
using Cameras;
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
        private const float Yaddition = 0.5f;

        private readonly IGameStateMachine _gameStateMachine;
        private readonly ISceneLoader _sceneLoader;
        private readonly IStaticDataService _staticDataService;
        private readonly ISaveLoadService _saveLoadService;
        private readonly IPlayerProgressService _playerProgressService;
        private SceneId _sceneId;
        private bool _isInitial = true;
        private GameObject _tank;
        private IEnemiesContainer _enemiesContainer;
        private IEnemiesGenerator _enemiesGenerator;
        private IUIContainer _uiContainer;
        private LevelStaticData _levelStaticData;
        private TankMovement _tankMovement;
        private IGameObjectsMover _gameObjectsMover;
        private TankRotation _tankRotation;
        private TankShooting _tankShooting;
        private TankWeaponChanger _tankWeaponChanger;

        public LoadSceneState(IGameStateMachine gameGameStateMachine, ISceneLoader sceneLoader,
            IPlayerProgressService playerProgressService, ISaveLoadService saveLoadService,
            IStaticDataService staticDataService, GameObject tank, IUIContainer uiContainer,
            IEnemiesContainer enemiesContainer, IEnemiesGenerator enemiesGenerator,
            IGameObjectsMover gameObjectsMover)
        {
            _enemiesGenerator = enemiesGenerator;
            _gameObjectsMover = gameObjectsMover;
            _gameStateMachine = gameGameStateMachine;
            _sceneLoader = sceneLoader;
            _playerProgressService = playerProgressService;
            _saveLoadService = saveLoadService;
            _staticDataService = staticDataService;
            _tank = tank;
            _uiContainer = uiContainer;
            _enemiesContainer = enemiesContainer;
            _tankMovement = _tank.GetComponent<TankMovement>();
            _tankRotation = _tank.GetComponentInChildren<TankRotation>();
            _tankShooting = _tank.GetComponentInChildren<TankShooting>();
            _tankWeaponChanger = _tank.GetComponentInChildren<TankWeaponChanger>();
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
            _enemiesContainer.GameObject.SetActive(false);
            _enemiesGenerator.GameObject.SetActive(false);
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
                CameraFollow(_tank);
            }
        }

        private void InitializeUIRoot(int victoryCount)
        {
            _uiContainer.GameObject.SetActive(true);
            _uiContainer.StartGameWindow.Construct(_gameObjectsMover, _enemiesGenerator, _uiContainer.GameScreen,
                _tankMovement, _tankRotation, _tankShooting, _tankWeaponChanger);
            _uiContainer.GameOverWindow.Construct(_enemiesGenerator, _tankMovement, _tankRotation, _tankShooting,
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
            _enemiesGenerator.Construct(_enemiesContainer, levelData.Enemy1Count, levelData.Enemy2Count,
                levelData.SecondsBetweenSpawns, levelData.EnemySpawners, _tank.transform);
            _gameObjectsMover.Construct(_enemiesGenerator);
            _enemiesContainer.GameObject.SetActive(true);
        }

        private void InstantiateGameObjects()
        {
            _tank.SetActive(true);
        }

        private void InitializeGameObjects()
        {
            _tankMovement.Construct(_levelStaticData.InitialTankPosition.AddY(Yaddition));
            _tankRotation.Construct();
            _tankWeaponChanger.Construct();
            _tankShooting.Construct(_tankWeaponChanger);
            _tank.GetComponent<TankHealth>().Construct(_tankMovement, _tank.GetComponent<TankDeath>(),
                _tank.GetComponent<TankHit>(), _gameObjectsMover, _uiContainer.GameOverWindow);
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

        private void CameraFollow(GameObject tank) =>
            Camera.main.GetComponent<CameraTankFollower>().Follow(tank);
    }
}