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

    public async Task DeleteUser(int userId)
    {
        try
        {
            var player = await dbContext.Players.FindAsync(userId);
            dbContext.Players.Remove(player);
            await dbContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            throw new ArgumentException("Player not found");
        }
    }
}