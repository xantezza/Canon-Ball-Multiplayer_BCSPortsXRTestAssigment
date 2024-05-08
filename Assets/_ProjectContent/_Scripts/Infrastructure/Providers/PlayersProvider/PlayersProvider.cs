using System.Collections.Generic;
using Gameplay.Player;

namespace Infrastructure.Providers.PlayersProvider
{
    public class PlayersProvider : IPlayersProvider
    {
        public HashSet<Canon> Players { get; } = new();

        public void RegisterPlayer(Canon canon)
        {
            Players.Add(canon);
        }

        public void UnregisterPlayer(Canon canon)
        {
            Players.Remove(canon);
        }
    }
}