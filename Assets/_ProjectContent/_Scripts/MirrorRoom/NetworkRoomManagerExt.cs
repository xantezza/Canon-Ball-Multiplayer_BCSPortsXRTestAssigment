using Mirror;
using UnityEngine;

namespace MirrorRoom
{
    public class NetworkRoomManagerExt : NetworkRoomManager
    {
        bool showStartButton;

        public override void OnRoomServerPlayersReady()
        {
            if (Mirror.Utils.IsHeadless())
            {
                base.OnRoomServerPlayersReady();
            }
            else
            {
                showStartButton = true;
            }
        }

        public override void OnGUI()
        {
            base.OnGUI();

            if (allPlayersReady && showStartButton && GUI.Button(new Rect(150, 300, 120, 20), "START GAME"))
            {
                showStartButton = false;
                ServerChangeScene(GameplayScene);
            }
        }
    }
}