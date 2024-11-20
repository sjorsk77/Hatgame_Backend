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
    
    public GameController(IGameService gameService)
    {
        _gameService = gameService;
    }
    
    [HttpPost("create")]
    public async Task<IActionResult> CreateGame(CreateGameRequest request)
    {
        var newGame = await _gameService.CreateGameAsync(request);

        return Ok(newGame);
    }
    
    [HttpPost("update-score")]
    public async Task<IActionResult> UpdateScore(int userId, int score)
    {
        if (score < 0) return BadRequest("Score must be a non-negative value.");
        
        try
        {
            var updatedGame = await _gameService.UpdateGameScoreAsync(userId, score);

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

        return Ok(joinedGame);
    }
}