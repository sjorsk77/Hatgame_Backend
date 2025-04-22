using DLL.Entities;
using DLL.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class GameRepository(ApplicationDbContext dbContext) : IGameRepository
{
    public async Task<Game> UpdateGameScoreAsync(int userId, int newScore)
    {
        var player = await dbContext.Players.FindAsync(userId);
        
        if (player == null)
            throw new ArgumentException("Player not found");
        
        player.Score = newScore;
        
        await dbContext.SaveChangesAsync();
        
        return (await dbContext.Games
            .Include(g => g.Players)
            .FirstOrDefaultAsync(g => g.Id == player.GameId))!;
    }

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

    public async Task<Game> UpdateGameAsync(Game game)
    {
        var updatedGame =  dbContext.Games.Update(game);
        
        await dbContext.SaveChangesAsync();
        
        return (await dbContext.Games
            .Include(g => g.Players)
            .FirstOrDefaultAsync(g => g.Id == updatedGame.Entity.Id))!;
    }

    public async Task<List<Game>> GetAllGamesAsync()
    {
        return await dbContext.Games.ToListAsync();
    }
}