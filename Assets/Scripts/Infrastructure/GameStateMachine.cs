using System;
using System.Collections.Generic;
using Infrastructure.States;
using Pool;
using Services;
using Services.PersistentProgress;
using Services.SaveLoad;
using Services.StaticData;
using UI.Screens.GameLoop;
using UnityEngine;

namespace Infrastructure
{
    public class GameStateMachine : IGameStateMachine
    {
        private readonly Dictionary<Type, IExitableState> _states;
        private IExitableState _activeState;

        public GameStateMachine(ISceneLoader sceneLoader, AllServices services, GameObject tank,
            IUIContainer uiContainer, IEnemiesContainer enemiesContainer,
            IEnemiesGenerator enemiesGenerator,
            IGameObjectsMover gameObjectsMover)
        {
            _states = new Dictionary<Type, IExitableState>()
            {
                [typeof(BootstrapState)] = new BootstrapState(this, sceneLoader, services),
                [typeof(LoadSceneState)] =
                    new LoadSceneState(this, sceneLoader, services.Single<IPlayerProgressService>(),
                        services.Single<ISaveLoadService>(), services.Single<IStaticDataService>(), tank, uiContainer,
                        enemiesContainer, enemiesGenerator, gameObjectsMover),
                [typeof(LoadPlayerProgressState)] =
                    new LoadPlayerProgressState(this, services.Single<IPlayerProgressService>(),
                        services.Single<ISaveLoadService>()),
                [typeof(GameLoopState)] = new GameLoopState(this),
            };
        }

        public void Enter<TState>() where TState : class, IState
        {
            TState state = ChangeState<TState>();
            state.Enter();
        }

        public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadedState<TPayload>
        {
            TState state = ChangeState<TState>();
            state.Enter(payload);
        }

        private TState ChangeState<TState>() where TState : class, IExitableState
        {
            _activeState?.Exit();

            TState state = GetState<TState>();
            _activeState = state;

            return state;
        }

        private TState GetState<TState>() where TState : class, IExitableState =>
            _states[typeof(TState)] as TState;
    }
}