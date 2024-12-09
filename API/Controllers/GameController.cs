using DLL.IServices;
using DLL.RequestModels;
using Erdogan_Backend.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Erdogan_Backend.Controllers;

[ApiController]
[Route("[controller]")]
public class GameController : Controller
{
    private readonly IGameService _gameService;
    private readonly IHubContext<GameHub> _gameHub;
    
    public GameController(IGameService gameService, IHubContext<GameHub> gameHub)
    {
        _gameService = gameService;
        _gameHub = gameHub;
    }
    
    [HttpPost("create")]
    public async Task<IActionResult> CreateGame(CreateGameRequest request)
    {
        var newGame = await _gameService.CreateGameAsync(request);
        var allGames = await _gameService.GetAllGamesAsync();

        await _gameHub.Clients.All.SendAsync("GameList", allGames);

        return Ok(newGame);
    }
    
    [HttpPost("update-score")]
    public async Task<IActionResult> UpdateScore(int userId, int score)
    {
        if (score < 0) return BadRequest("Score must be a non-negative value.");
        
        try
        {
            var updatedGame = await _gameService.UpdateGameScoreAsync(userId, score);

            await _gameHub.Clients.All.SendAsync("ReceiveGame", updatedGame);
            
            return Ok(updatedGame);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
    
    [HttpPost("join")]
    public async Task<IActionResult> JoinGame(JoinGameRequest request)
    {
        var joinedGame = await _gameService.JoinGameAsync(request);
        
        await _gameHub.Clients.Group(joinedGame.Id.ToString()).SendAsync("ReceiveGame", joinedGame);

        return Ok(joinedGame);
    }
    
    [HttpPost("leave")]
    public async Task<IActionResult> LeaveGame(int playerId)
    {
        var game = await _gameService.LeaveGameAsync(playerId);
        
        await _gameHub.Clients.Group($"Game-{game.Id}").SendAsync("ReceiveGame", game);

        return Ok(game);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllGames()
    {
        var games = await _gameService.GetAllGamesAsync();

        return Ok(games);
    }
    [HttpGet("{gameId}")]
    public async Task<IActionResult> GetGameById(int gameId)
    {
        var game = await _gameService.GetGameByIdAsync(gameId);

        return Ok(game);
    }
}