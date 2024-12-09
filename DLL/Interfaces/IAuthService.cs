namespace DLL.Interfaces;

public interface IAuthService
{
    string Login(string email, string password);
}