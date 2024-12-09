using DLL.Entities;

namespace DLL.Interfaces;

public interface ITokenService
{
    string GenerateAdminToken(Admin admin);
}