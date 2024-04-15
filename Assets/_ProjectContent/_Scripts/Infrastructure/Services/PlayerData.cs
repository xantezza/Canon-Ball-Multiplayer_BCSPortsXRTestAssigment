using System;
using Mirror;
using Mirror.Examples.NetworkRoom;
using UnityEngine;

namespace Infrastructure.Services
{
    [Serializable]
    public class PlayerData
    {
        public Color PlayerColor = Color.white; 
        [SyncVar]
        public NetworkRoomPlayerExt.ColorEnum PlayerColorEnum = NetworkRoomPlayerExt.ColorEnum.None;
        public string Name;
        public int Index;
    }
}