using System.ComponentModel.DataAnnotations;

namespace DLL.RequestModels;

public class JoinGameReq
{
    [RegularExpression(@"^\d{6}$", ErrorMessage = "The PIN must be exactly 6 digits.")]
    public int GamePin { get;  set; }
    public string PlayerName { get;  set; } = String.Empty;
}