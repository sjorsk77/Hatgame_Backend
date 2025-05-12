using DLL.Interfaces;
using DLL.RequestModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Erdogan_Backend.Controllers;

[Authorize(Roles = "Admin")]
[ApiController]
[Route("[controller]")]
public class AdminController : Controller
{
    private readonly IAuthService _authService;
    
    public AdminController(IAuthService authService)
    {
        _authService = authService;
    }

    /*[HttpPost("create")]
    public async Task<IActionResult> Create(AdminCreateRequest request)
    {
        var admin = await _authService.CreateAdminAsync(request);
        
        return Ok(admin);
    }*/
    
}