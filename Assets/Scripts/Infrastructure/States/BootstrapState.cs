using Services;
using Services.PersistentProgress;
using Services.SaveLoad;
using Services.StaticData;
using UnityEngine;

namespace Infrastructure.States
{
    public class BootstrapState : IState
    {
        private readonly IGameStateMachine _stateMachine;
        private readonly ISceneLoader _sceneLoader;
        private AllServices _services;

        public BootstrapState(IGameStateMachine stateMachine, ISceneLoader sceneLoader, AllServices services)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _services = services;

            RegisterServices();
            SetTargetFrameRate();
        }

        public void Enter() =>
            _sceneLoader.Load(nextSceneId: SceneId.Initial, onLoaded: EnterLoadLevel);

        private void EnterLoadLevel() =>
            _stateMachine.Enter<LoadPlayerProgressState>();

        private void RegisterServices()
        {
            _services.RegisterSingle<IGameStateMachine>(_stateMachine);
            _services.RegisterSingle<IPlayerProgressService>(new PlayerProgressService());
            _services.RegisterSingle<ISaveLoadService>(new SaveLoadService(_services.Single<IPlayerProgressService>()));
            StaticDataService staticDataService = new StaticDataService();
            staticDataService.Load();
            _services.RegisterSingle<IStaticDataService>(staticDataService);
        }

        public void Exit()
        {
        }

        private void SetTargetFrameRate()
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = Screen.currentResolution.refreshRate;
        }
    }
}