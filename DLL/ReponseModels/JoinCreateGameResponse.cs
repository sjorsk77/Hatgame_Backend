using DLL.Entities;

namespace DLL.ReponseModels;

public class JoinCreateGameResponse
{
    public Game Game { get; set; }
    public string Token { get; set; }
}