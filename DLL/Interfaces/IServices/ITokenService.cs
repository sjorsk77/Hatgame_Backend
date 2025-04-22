using System.Security.Claims;
using DLL.Entities;

namespace DLL.Interfaces;

public interface ITokenService
{
    string GenerateAdminToken(Admin admin);
    string GeneratePlayerToken(Player player);
    ClaimsPrincipal ExtractClaims(string token);
}