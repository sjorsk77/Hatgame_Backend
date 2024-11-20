using DLL.Entities;
using DLL.Interfaces;

namespace DAL.Repositories;

public class PlayerRepository(ApplicationDbContext dbContext) : IPlayerRepository
{
    
    public async Task<Player> CreatePlayerAsync(Player player)
    {
        var createdPlayer = await dbContext.Players.AddAsync(player);
        
        await dbContext.SaveChangesAsync();
        
        return createdPlayer.Entity;
    }
}