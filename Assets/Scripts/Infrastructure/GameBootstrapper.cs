using Infrastructure.States;
using Pool;
using UI.Screens.GameLoop;
using UnityEngine;

namespace Infrastructure
{
    public class GameBootstrapper : MonoBehaviour, ICoroutineRunner
    {
        [SerializeField] private GameObject _tankPrefab;
        [SerializeField] private GameObject _uiContainerPrefab;
        [SerializeField] private GameObject _objectsContainerPrefab;
        [SerializeField] private GameObject _objectsGeneratorPrefab;

        private GameObject _tank;
        private UIContainer _uiContainer;
        private GameObject _objectsContainer;
        private GameObject _objectsGenerator;
        private Game _game;

        private void Awake()
        {
            DontDestroyOnLoad(this);

            if (_tank == null)
                _tank = Instantiate(_tankPrefab, Vector2.zero, Quaternion.identity);

            _tank.SetActive(false);

            if (_objectsContainer == null)
                _objectsContainer = Instantiate(_objectsContainerPrefab);

            _objectsContainer.SetActive(false);

            if (_objectsGenerator == null)
                _objectsGenerator = Instantiate(_objectsGeneratorPrefab);

            _objectsGenerator.SetActive(false);

            if (_uiContainer == null)
                _uiContainer = Instantiate(_uiContainerPrefab).GetComponent<UIContainer>();

            _uiContainer.gameObject.SetActive(false);

            _game = new Game(this, tank: _tank,
                uiContainer: _uiContainer.GetComponent<UIContainer>(),
                enemiesContainer: _objectsContainer.GetComponent<EnemiesContainer>(),
                enemiesGenerator: _objectsGenerator.GetComponent<EnemiesGenerator>(),
                gameObjectsMover: _objectsGenerator.GetComponent<GameGameObjectsMover>());
            _game.StateMachine.Enter<BootstrapState>();
        }
    }
}