using Infrastructure.Factories;
using Infrastructure.Providers.PlayersProvider;
using UI.Gameplay;
using UnityEngine;
using Zenject;

namespace Infrastructure.Installers.GameplayContext
{
    public class GameplayInstaller : MonoInstaller
    {
        [SerializeField] private SoccerBallFactoryMirror _soccerBallFactoryMirror;

        public override void InstallBindings()
        {
            BindSoccerBallFactory();
            BindPlayersProvider();
        }

        private void BindPlayersProvider()
        {
            Container.BindInterfacesTo<PlayersProvider>().FromNew().AsSingle().NonLazy();
        }

        private void BindSoccerBallFactory()
        {
            Container.BindInterfacesTo<SoccerBallFactoryMirror>().FromInstance(_soccerBallFactoryMirror).AsSingle().NonLazy();
        }
    }
}