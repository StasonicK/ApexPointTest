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
            IObjectsContainer objectsContainer, IObjectsGenerator objectsGenerator, IObjectsMover objectsMover)
        {
            StateMachine =
                new GameStateMachine(new SceneLoader(coroutineRunner), AllServices.Container, tank: tank,
                    uiContainer: uiContainer, objectsContainer, objectsGenerator, objectsMover);
        }

        private void OnDestroy() =>
            AllServices.Container.Single<ISaveLoadService>().ClearData();
    }
}