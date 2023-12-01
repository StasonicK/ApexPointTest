using Data;
using Services.PersistentProgress;
using Services.SaveLoad;

namespace Infrastructure.States
{
    public class LoadPlayerProgressState : IState
    {
        private const SceneId InitialScene = SceneId.Level_1;

        private readonly IGameStateMachine _stateMachine;
        private readonly ISaveLoadService _saveLoadService;
        private readonly IPlayerProgressService _playerProgressService;

        public LoadPlayerProgressState(IGameStateMachine stateMachine, IPlayerProgressService playerProgressService,
            ISaveLoadService saveLoadService)
        {
            _stateMachine = stateMachine;
            _playerProgressService = playerProgressService;
            _saveLoadService = saveLoadService;
        }

        public void Enter()
        {
            LoadProgressOrInitNew();
            _stateMachine.Enter<LoadSceneState, SceneId>(InitialScene);
        }

        public void Exit()
        {
        }

        private void LoadProgressOrInitNew()
        {
            // SettingsData settingsData = _saveLoadService.LoadSettingsData();
            // _playerProgressService.SetSettingsData(settingsData ?? NewSettingsData());
            // GameData gameData = _saveLoadService.LoadGameData();
            // _playerProgressService.SetGameData(gameData ?? NewGameData());
            _playerProgressService.SetSettingsData(NewSettingsData());
            _playerProgressService.SetGameData(NewGameData());
        }

        private SettingsData NewSettingsData() =>
            new SettingsData(InitialScene);

        private GameData NewGameData() =>
            new GameData(InitialScene);
    }
}