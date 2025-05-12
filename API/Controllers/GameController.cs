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
    public async Task<IActionResult> CreateGame(string playerName)
    {
        var createGameResponse = await _gameService.CreateGameAsync(playerName);

        return Ok(createGameResponse);
    }
    
    [HttpPost("join")]
    public async Task<IActionResult> JoinGame(JoinGameReq request)
    {
        var joinedGameResponse = await _gameService.JoinGameAsync(request);

        return Ok(joinedGameResponse);
    }
}