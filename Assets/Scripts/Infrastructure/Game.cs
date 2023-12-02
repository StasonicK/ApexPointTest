using Pool;
using Pool.Enemies;
using Pool.Projectiles;
using Services;
using Services.SaveLoad;
using UI.Screens.GameLoop;
using UnityEngine;

namespace Infrastructure
{
    public class Game : MonoBehaviour
    {
        [HideInInspector] public GameStateMachine StateMachine;

        public Game(ICoroutineRunner coroutineRunner, GameObject tank, IUIContainer uiContainer,
            IEnemiesContainer enemiesContainer, IProjectilesContainer projectilesContainer,
            IEnemiesGenerator enemiesGenerator, IProjectilesGenerator projectilesGenerator,
            IGameObjectsMover gameObjectsMover)
        {
            StateMachine =
                new GameStateMachine(new SceneLoader(coroutineRunner), AllServices.Container, tank: tank,
                    uiContainer: uiContainer, enemiesContainer, projectilesContainer, enemiesGenerator,
                    projectilesGenerator, gameObjectsMover);
        }

        private void OnDestroy() =>
            AllServices.Container.Single<ISaveLoadService>().ClearData();
    }
}