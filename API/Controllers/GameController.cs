using System.Security.Claims;
using DLL.IServices;
using DLL.RequestModels;
using Erdogan_Backend.Hubs;
using Microsoft.AspNetCore.Authorization;
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
        var createGameResponse = await _gameService.CreateGameAsync(request);
        var liveGames = await _gameService.GetLiveMatchesAsync();

        await _gameHub.Clients.All.SendAsync("GameList", liveGames);

        return Ok(createGameResponse);
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
        var joinedGameResponse = await _gameService.JoinGameAsync(request);
        
        await _gameHub.Clients.Group(joinedGameResponse.Game.Id.ToString()).SendAsync("ReceiveGame", joinedGameResponse);

        return Ok(joinedGameResponse);
    }
    
    [Authorize]
    [HttpGet("leave/{gameId}")]
    public async Task<IActionResult> LeaveGame(int gameId)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? 
                               throw new Exception("Could not find current user id in token"));
        var game = await _gameService.LeaveGameAsync(gameId, userId);
        
        await _gameHub.Clients.Group($"Game-{game.Id}").SendAsync("ReceiveGame", game);

        return Ok(game);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllGames()
    {
        var games = await _gameService.GetAllGamesAsync();

        return Ok(games);
    }
    
    [HttpGet("live")]
    public async Task<IActionResult> GetLiveMatches()
    {
        var liveGames = await _gameService.GetLiveMatchesAsync();

        return Ok(liveGames);
    }
    
    [HttpGet("{gameId}")]
    public async Task<IActionResult> GetGameById(int gameId)
    {
        var game = await _gameService.GetGameByIdAsync(gameId);

        return Ok(game);
    }
    
    [Authorize(Roles = "Host")]
    [HttpPost("start/{gameId}")]
    public async Task<IActionResult> StartGame(int gameId)
    {
        var game = await _gameService.GetGameByIdAsync(gameId);
        
        await _gameHub.Clients.Group($"Game-{game.Id}").SendAsync("StartGame", game);

        return Ok(game);
    }
}