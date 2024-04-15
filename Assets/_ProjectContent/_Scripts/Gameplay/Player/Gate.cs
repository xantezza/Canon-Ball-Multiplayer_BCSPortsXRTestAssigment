using Infrastructure.Factories;
using Mirror;
using UnityEngine;
using Zenject;

namespace Gameplay.Player
{
    public class Gate : NetworkBehaviour
    {
        private SoccerBallFactory _soccerBallFactory;

        [Inject]
        private void Inject(SoccerBallFactory soccerBallFactory)
        {
            _soccerBallFactory = soccerBallFactory;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<SoccerBall.SoccerBall>(out var ball))
            {
                if (isServer) ball.AddScoreToOwner();
                
                _soccerBallFactory.Despawn(ball);
            }
        }
    }
}