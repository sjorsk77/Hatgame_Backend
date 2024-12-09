using DAL.Repositories;
using DLL.Interfaces;
using DLL.IRepositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public static class DependencyInjection
{
    public static IServiceCollection AddDal(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
        
        services.AddScoped<IGameRepository, GameRepository>();
        services.AddScoped<IPlayerRepository, PlayerRepository>();
        return services;
    }
}