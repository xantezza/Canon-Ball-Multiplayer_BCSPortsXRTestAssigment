using MirrorRoom;

namespace Infrastructure.Providers.PlayerDataProvider
{
    public interface IPlayerDataProvider
    {
        PlayerData PlayerData { get; set; }
    }
}