using Infrastructure.Services.Modals;
using Infrastructure.StateMachines;
using Infrastructure.StateMachines.GameLoopStateMachine;
using Infrastructure.StateMachines.InitializationStateMachine;
using UnityEngine;
using Zenject;

namespace Infrastructure.Installers.ProjectContext
{
    public class ProjectContextFactoryInstaller : MonoInstaller
    {
        [SerializeField] private ModalsFactory _modalsFactory;

        public override void InstallBindings()
        {
            BindStatesFactory();
            BindGameLoopStateMachineFactory();
            BindInitializationStateMachineFactory();
            BindModalsFactory();
        }

        private void BindModalsFactory()
        {
            Container.Bind<ModalsFactory>().FromInstance(_modalsFactory).AsSingle().NonLazy();
        }

        private void BindInitializationStateMachineFactory()
        {
            Container.Bind<InitializationStateMachineFactory>().FromNew().AsSingle().NonLazy();
        }

        private void BindGameLoopStateMachineFactory()
        {
            Container.Bind<GameLoopStateMachineFactory>().FromNew().AsSingle().NonLazy();
        }

        private void BindStatesFactory()
        {
            Container.Bind<StatesFactory>().FromNew().AsSingle().NonLazy();
        }
    }
}