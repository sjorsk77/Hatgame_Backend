using DLL.Entities;
using DLL.RequestModels;

namespace DLL.Interfaces;

public interface IAuthService
{
    Task<Admin> AdminLoginAsync(AdminLoginRequest request);
    Task<Admin> CreateAdminAsync(AdminCreateRequest request);
}