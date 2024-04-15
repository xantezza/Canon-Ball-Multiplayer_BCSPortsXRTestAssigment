using System;
using Gameplay.SoccerBall;
using Infrastructure.Factories;
using UnityEngine;
using Zenject;

namespace Infrastructure.Installers.ProjectContext
{
    public class MonoMemoryPoolInstaller : MonoInstaller
    {
        [Serializable]
        public class PoolContainer<T> where T : MonoBehaviour
        {
            public T Prefab;
            public int InitialSize;
            public string PoolGroupName;
        }

        [SerializeField] private PoolContainer<SoccerBall> _soccerBallPoolContainer;

        public override void InstallBindings()
        {
            BindWithDataContainer<SoccerBall, SoccerBallFactory.Pool>(_soccerBallPoolContainer);
            Container.Bind<SoccerBallFactory>().FromNew().AsSingle().NonLazy();
        }

        private void BindWithDataContainer<T1, T2>(PoolContainer<T1> container)
            where T1 : MonoBehaviour
            where T2 : IMemoryPool
        {
            Container.BindMemoryPool<T1, T2>()
                .WithInitialSize(container.InitialSize)
                .FromComponentInNewPrefab(container.Prefab)
                .UnderTransformGroup(container.PoolGroupName)
                .NonLazy();
        }
    }
}