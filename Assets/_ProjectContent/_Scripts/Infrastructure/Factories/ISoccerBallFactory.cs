using Gameplay.SoccerBall;
using UnityEngine;

namespace Infrastructure.Factories
{
    public interface ISoccerBallFactory
    {
        SoccerBall Create(Vector3 position, Quaternion rotation);
        void Despawn(SoccerBall soccerBall);
    }
}