using System;
using Infrastructure.Factories;
using UnityEngine;
using Zenject;

namespace Infrastructure.Installers.GameplayContext
{
    [Serializable]
    public class GameplayFactoryInstaller : MonoInstaller
    {
        [SerializeField] private SoccerBallFactoryMirror _soccerBallFactoryMirror;

        public override void InstallBindings()
        {
            
            Container.Bind<SoccerBallFactoryMirror>().FromInstance(_soccerBallFactoryMirror).AsSingle().NonLazy();
        }
    }
}