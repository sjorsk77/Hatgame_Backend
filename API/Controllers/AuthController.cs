using DLL.Interfaces;
using DLL.RequestModels;
using Microsoft.AspNetCore.Mvc;

namespace Erdogan_Backend.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : Controller
{
    private readonly IAuthService _authService;
    private readonly ITokenService _tokenService;
    
    public AuthController(IAuthService authService, ITokenService tokenService)
    {
        _authService = authService;
        _tokenService = tokenService;
    }
    
    /*[HttpPost("login")]
    public async Task<IActionResult> Login(AdminLoginRequest request)
    {
        var admin = await _authService.AdminLoginAsync(request);
        if (admin == null) return Unauthorized("Invalid credentials.");
        
        var token = _tokenService.GenerateAdminToken(admin);
        return Ok(new { token });
    }*/
}