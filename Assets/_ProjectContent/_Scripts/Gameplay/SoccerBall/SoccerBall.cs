using Gameplay.Player;
using Infrastructure.Factories;
using Mirror;
using UnityEngine;
using Zenject;

namespace Gameplay.SoccerBall
{
    public class SoccerBall : NetworkBehaviour
    {
        private const float MAX_LIFETIME = 10;

        [SerializeField] private Rigidbody _rigidBody;

        private Canon _cachedOwnersCanon;
        private SoccerBallFactoryMirror _soccerBallFactory;
        private float _lifetime;

        [Inject]
        private void Inject(SoccerBallFactoryMirror soccerBallFactory)
        {
            _soccerBallFactory = soccerBallFactory;
        }

        public void Awake()
        {
            _rigidBody.Sleep();
        }

        public void Init(Canon canon, float impulseForce)
        {
            _rigidBody.velocity = Vector3.zero;
            _rigidBody.angularVelocity = Vector3.zero;
            _lifetime = 0;

            _cachedOwnersCanon = canon;
            ApplyForce(transform.forward * impulseForce);
        }

        private void Update()
        {
            _lifetime += Time.deltaTime;
            if (_lifetime > MAX_LIFETIME)
            {
                _soccerBallFactory.Despawn(this);
            }
        }

        private void ApplyForce(Vector3 force)
        {
            _rigidBody.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
        }

        [Server]
        public void AddScoreToOwner()
        {
            _cachedOwnersCanon.AddScore();
        }
    }
}