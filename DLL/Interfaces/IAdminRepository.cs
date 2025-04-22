using DLL.Entities;

namespace DLL.Interfaces;

public interface IAdminRepository
{
    Task<Admin> GetAdminByEmailAsync(string email);
    Task<Admin> CreateAdminAsync(Admin admin);
}