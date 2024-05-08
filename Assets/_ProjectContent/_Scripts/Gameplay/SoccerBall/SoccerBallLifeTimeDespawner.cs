using Infrastructure.Factories;
using UnityEngine;
using Zenject;

namespace Gameplay.SoccerBall
{
    public class SoccerBallLifeTimeDespawner : MonoBehaviour
    {
        private const float MAX_LIFETIME = 10;
        
        private ISoccerBallFactory _soccerBallFactory;
        private float _lifetime;

        [SerializeField] private SoccerBall _soccerBall;
        
        [Inject]
        private void Inject(ISoccerBallFactory soccerBallFactory)
        {
            _soccerBallFactory = soccerBallFactory;
        }

        private void OnEnable()
        {
            _lifetime = 0;
        }

        private void Update()
        {
            _lifetime += Time.deltaTime;
            if (_lifetime > MAX_LIFETIME)
            {
                _soccerBallFactory.Despawn(_soccerBall);
            }
        }
    }
}