using System;
using Mirror;
using UnityEngine;

namespace MirrorRoom
{
    [Serializable]
    public class PlayerData
    {
        public Color PlayerColor = Color.white;
        public string Name;
        public int Index;


        [SyncVar]
        public NetworkRoomPlayerExt.ColorEnum PlayerColorEnum = NetworkRoomPlayerExt.ColorEnum.None;
    }
}