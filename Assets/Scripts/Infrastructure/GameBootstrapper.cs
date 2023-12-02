using Infrastructure.States;
using Pool;
using Pool.Enemies;
using Pool.Projectiles;
using UI.Screens.GameLoop;
using UnityEngine;

namespace Infrastructure
{
    public class GameBootstrapper : MonoBehaviour, ICoroutineRunner
    {
        [SerializeField] private GameObject _tankPrefab;
        [SerializeField] private GameObject _uiContainerPrefab;
        [SerializeField] private GameObject _enemiesContainerPrefab;
        [SerializeField] private GameObject _projectilesContainerPrefab;
        [SerializeField] private GameObject _enemiesGeneratorPrefab;
        [SerializeField] private GameObject _projectilesGeneratorPrefab;

        private GameObject _tank;
        private UIContainer _uiContainer;
        private GameObject _enemiesContainer;
        private GameObject _projectilesContainer;
        private GameObject _enemiesGenerator;
        private GameObject _projectilesGenerator;
        private Game _game;

        private void Awake()
        {
            DontDestroyOnLoad(this);

            if (_tank == null)
                _tank = Instantiate(_tankPrefab, Vector2.zero, Quaternion.identity);

            _tank.SetActive(false);

            if (_enemiesContainer == null)
                _enemiesContainer = Instantiate(_enemiesContainerPrefab);

            // _enemiesContainer.SetActive(false);

            if (_enemiesGenerator == null)
                _enemiesGenerator = Instantiate(_enemiesGeneratorPrefab);

            _enemiesGenerator.SetActive(false);

            if (_projectilesContainer == null)
                _projectilesContainer = Instantiate(_projectilesContainerPrefab);

            // _projectilesContainer.SetActive(false);

            if (_projectilesGenerator == null)
                _projectilesGenerator = Instantiate(_projectilesGeneratorPrefab);

            _projectilesGenerator.SetActive(false);

            if (_uiContainer == null)
                _uiContainer = Instantiate(_uiContainerPrefab).GetComponent<UIContainer>();

            _uiContainer.gameObject.SetActive(false);

            _game = new Game(this, tank: _tank,
                uiContainer: _uiContainer.GetComponent<UIContainer>(),
                enemiesContainer: _enemiesContainer.GetComponent<EnemiesContainer>(),
                enemiesGenerator: _enemiesGenerator.GetComponent<EnemiesGenerator>(),
                projectilesContainer: _projectilesContainer.GetComponent<ProjectilesContainer>(),
                projectilesGenerator: _projectilesGenerator.GetComponent<ProjectilesGenerator>(),
                gameObjectsMover: _enemiesGenerator.GetComponent<GameGameObjectsMover>());
            _game.StateMachine.Enter<BootstrapState>();
        }
    }
}