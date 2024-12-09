using DLL.Interfaces;
using DLL.IServices;
using DLL.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DLL;

public static class DependencyInjection
{
    public static IServiceCollection AddDLL(this IServiceCollection services)
    {
        services.AddScoped<IGameService, GameService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ITokenService, TokenService>();
        
        return services;
    }
}