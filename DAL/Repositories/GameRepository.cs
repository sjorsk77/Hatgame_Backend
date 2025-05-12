using DLL.Entities;
using DLL.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class GameRepository(ApplicationDbContext dbContext) : IGameRepository
{
    public async Task<Game> CreateGameAsync(Game game)
    {
        var createdGame = await dbContext.Games.AddAsync(game);
        
        await dbContext.SaveChangesAsync();
        
        return (await dbContext.Games
            .Include(g => g.Players)
            .FirstOrDefaultAsync(g => g.Id == createdGame.Entity.Id))!;
    }

    public async Task<Game> GetGameByIdAsync(int gameId)
    {
        var game = await dbContext.Games
            .Include(g => g.Players)
            .FirstOrDefaultAsync(g => g.Id == gameId);
    
        if (game == null)
            throw new ArgumentException("Game not found");
    
        return game;
    }

    public async Task<Game> GetGameByPinAsync(int gamePin)
    {
        var game = await dbContext.Games
            .Include(g => g.Players)
            .FirstOrDefaultAsync(g => g.Pin == gamePin);

        if (game == null)
            throw new ArgumentException("Game not found");

        return game;
    }
    
    public async Task<bool> GamePinExistsAsync(int gamePin)
    {
        return await dbContext.Games.AnyAsync(g => g.Pin == gamePin);
    }
}