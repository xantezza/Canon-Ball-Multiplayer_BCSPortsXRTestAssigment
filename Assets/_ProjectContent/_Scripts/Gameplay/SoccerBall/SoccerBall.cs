using System;
using Gameplay.Player;
using Mirror;
using UnityEngine;

namespace Gameplay.SoccerBall
{
    public class SoccerBall : NetworkBehaviour
    {
        
        [SerializeField] private Rigidbody _rigidBody;

        private Canon _cachedOwnersCanon;

        public void Awake()
        {
            _rigidBody.Sleep();
        }

        public void Init(Canon canon, float impulseForce)
        {
            _rigidBody.velocity = Vector3.zero;
            _rigidBody.angularVelocity = Vector3.zero;

            _cachedOwnersCanon = canon;
            ApplyForce(transform.forward * impulseForce);
        }

        private void ApplyForce(Vector3 force)
        {
            _rigidBody.AddForce(force, ForceMode.Impulse);
        }

        [Server]
        public void AddScoreToOwner()
        {
            _cachedOwnersCanon.AddToScore(1);
        }
    }
}