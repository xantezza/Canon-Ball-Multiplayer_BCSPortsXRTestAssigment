using DG.Tweening;
using Infrastructure.Factories;
using Mirror;
using UnityEngine;
using Zenject;

namespace Gameplay.Player
{
    public class Gate : NetworkBehaviour
    {
        [SerializeField] private Canon _ownerCanon;
        [SerializeField] private int _endValueX;
        [SerializeField] private int _duration;

        private ISoccerBallFactory _soccerBallFactory;
        private Sequence _sequence;

        [Inject]
        private void Inject(ISoccerBallFactory soccerBallFactory)
        {
            _soccerBallFactory = soccerBallFactory;
        }

        private void OnEnable()
        {
            _sequence = DOTween.Sequence();
            _sequence.Append(transform.DOLocalMoveX(-_endValueX, _duration));
            _sequence.Append(transform.DOLocalMoveX(_endValueX, _duration));
            _sequence.SetEase(Ease.Linear);
            _sequence.SetLoops(-1);
        }

        private void OnDisable()
        {
            _sequence?.Kill();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<SoccerBall.SoccerBall>(out var ball))
            {
                _soccerBallFactory.Despawn(ball);

                if (isServer)
                {
                    ball.AddScoreToOwner();
                    _ownerCanon.AddToScore(-1);
                }
            }
        }
    }
}