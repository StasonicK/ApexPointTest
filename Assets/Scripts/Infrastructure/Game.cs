using Pool;
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
            IGameObjectsContainer gameObjectsContainer, IGameObjectsGenerator gameObjectsGenerator,
            IGameObjectsMover gameObjectsMover)
        {
            StateMachine =
                new GameStateMachine(new SceneLoader(coroutineRunner), AllServices.Container, tank: tank,
                    uiContainer: uiContainer, gameObjectsContainer, gameObjectsGenerator, gameObjectsMover);
        }

        private void OnDestroy() =>
            AllServices.Container.Single<ISaveLoadService>().ClearData();
    }
}