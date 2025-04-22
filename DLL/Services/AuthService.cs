using DLL.Entities;
using DLL.Interfaces;
using DLL.RequestModels;
using BCrypt.Net;

namespace DLL.Services;

public class AuthService : IAuthService
{
    private readonly IAdminRepository _adminRepository;

    public AuthService(IAdminRepository adminRepository)
    {
        _adminRepository = adminRepository;
    }

    public async Task<Admin> AdminLoginAsync(AdminLoginRequest request)
    {
        var admin = await _adminRepository.GetAdminByEmailAsync(request.Email);

        if (admin == null || !BCrypt.Net.BCrypt.Verify(request.Password, admin.Password))
            throw new UnauthorizedAccessException("Invalid credentials");

        return admin;
    }

    public Task<Admin> CreateAdminAsync(AdminCreateRequest request)
    {
        var admin = new Admin
        {
            Email = request.Email,
            Password = BCrypt.Net.BCrypt.HashPassword(request.Password)
        };

        return _adminRepository.CreateAdminAsync(admin);
    }
}