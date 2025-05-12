using DLL.Entities;
using DLL.Interfaces;

namespace DLL.Services;

public class PlayerService : IPlayerService
{
    private readonly IPlayerRepository _playerRepository;
    
    public PlayerService(IPlayerRepository playerRepository)
    {
        _playerRepository = playerRepository;
    }
    
    public Task DeleteUser(int userId)
    {
        return _playerRepository.DeleteUser(userId);
    }
}