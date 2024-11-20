using DLL.Entities;

namespace DLL.Interfaces;

public interface IPlayerRepository
{
    Task<Player> CreatePlayerAsync(Player player);
}