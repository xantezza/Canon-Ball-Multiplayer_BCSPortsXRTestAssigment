using Gameplay.Player;
using Mirror;
using UnityEngine;

namespace Gameplay.SoccerBall
{
    public class SoccerBall : NetworkBehaviour
    {
        [SerializeField] Rigidbody _rigidBody;
        private Canon _cachedOwnersCanon;

        public void Init(Canon canon, float impulseForce)
        {
            _cachedOwnersCanon = canon;
            ApplyForce(transform.forward * impulseForce);
        }

        private void ApplyForce(Vector3 force)
        {
            _rigidBody.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
        }

        public void AddScoreToOwner()
        {
            _cachedOwnersCanon.AddScore();
        }
    }
}