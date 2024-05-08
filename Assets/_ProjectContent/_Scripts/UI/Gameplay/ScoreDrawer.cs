using Infrastructure.Providers.PlayersProvider;
using UnityEngine;
using Zenject;

namespace UI.Gameplay
{
    public class ScoreDrawer : MonoBehaviour, IScoreDrawer
    {
        private IPlayersProvider _playersProvider;

        [Inject]
        private void Inject(IPlayersProvider playersProvider)
        {
            _playersProvider = playersProvider;
        }

        private void OnGUI()
        {
            foreach (var player in _playersProvider.Players)
            {
                GUILayout.BeginArea(new Rect((150), (player.PlayerData.Index + 1) * 150, 140, 130f));

                GUILayout.Label($"{player.PlayerData.Name} ({player.PlayerData.PlayerColorEnum}): {player.Score.Value}");

                GUILayout.EndArea();
            }
        }
    }
}