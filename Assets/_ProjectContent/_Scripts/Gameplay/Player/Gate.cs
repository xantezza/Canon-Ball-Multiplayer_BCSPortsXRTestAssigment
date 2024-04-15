using UnityEngine;

namespace Gameplay.Player
{
    public class Gate : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            other.GetComponent<SoccerBall.SoccerBall>().AddScoreToOwner();
        }
    }
}