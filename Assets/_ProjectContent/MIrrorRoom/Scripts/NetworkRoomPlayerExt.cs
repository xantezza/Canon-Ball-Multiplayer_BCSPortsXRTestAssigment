using Infrastructure.Services;
using UnityEngine;
using Zenject;

namespace Mirror.Examples.NetworkRoom
{
    [AddComponentMenu("")]
    public class NetworkRoomPlayerExt : NetworkRoomPlayer
    {
        private PlayerColorTracker _playerColorTracker;

        [Inject]
        private void Inject(PlayerColorTracker playerColorTracker)
        {
            _playerColorTracker = playerColorTracker;
        }

        public override void OnGUI()
        {
            base.OnGUI();

            if (!isLocalPlayer) return;
            
            GUILayout.BeginArea(new Rect(20f + (index * 100), 600, 180f, 130f));
            if (GUILayout.Button("White"))
                _playerColorTracker.PlayerColor = Color.white;
            if (GUILayout.Button("Red"))
                _playerColorTracker.PlayerColor = Color.red;
            if (GUILayout.Button("Blue"))
                _playerColorTracker.PlayerColor = Color.blue;
            if (GUILayout.Button("Gold"))
                _playerColorTracker.PlayerColor = Color.yellow;
            
            GUILayout.EndArea();
        }
    }
}
