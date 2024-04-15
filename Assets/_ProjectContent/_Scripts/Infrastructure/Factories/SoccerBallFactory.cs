using System.Linq;
using Gameplay.Player;
using Gameplay.SoccerBall;
using UnityEngine;
using Zenject;

namespace Infrastructure.Factories
{
    public class SoccerBallFactory
    {
        public struct SoccerBallData
        {
            public Vector3 Position;
            public Quaternion Rotation;
            public Canon Canon;
            public float ShootForce;
        }

        public class Pool : MonoMemoryPool<SoccerBallData, SoccerBall>
        {
            protected override void Reinitialize(SoccerBallData data, SoccerBall item)
            {
                item.transform.SetPositionAndRotation(data.Position, data.Rotation);
                item.Init(data.Canon, data.ShootForce);
            }
        }

        private Pool _pool;

        [Inject]
        public SoccerBallFactory(Pool pool)
        {
            _pool = pool;
        }

        public SoccerBall Create(SoccerBallData data)
        {
            return _pool.Spawn(data);
        }

        public void Despawn(SoccerBall soccerBall)
        {
            if (_pool == null || _pool.InactiveItems.Contains(soccerBall)) return;
            _pool.Despawn(soccerBall);
        }
    }
}