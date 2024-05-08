using System.Collections.Generic;
using Gameplay.Player;

namespace Infrastructure.Providers.PlayersProvider
{
    public interface IPlayersProvider
    {
        HashSet<Canon> Players { get; }
        void RegisterPlayer(Canon canon);
        void UnregisterPlayer(Canon canon);
    }
}