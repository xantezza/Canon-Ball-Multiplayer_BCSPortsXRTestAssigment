using Mirror;
using MirrorRoom;
using UnityEngine;

namespace Infrastructure.Providers.PlayerDataProvider
{
    public class PlayerData
    {
        public Color PlayerColor = Color.white;
        public string Name;
        public int Index;


        [SyncVar]
        public NetworkRoomPlayerExt.ColorEnum PlayerColorEnum = NetworkRoomPlayerExt.ColorEnum.None;
    }
}