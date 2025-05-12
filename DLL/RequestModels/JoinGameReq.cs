using System.ComponentModel.DataAnnotations;

namespace DLL.RequestModels;

public class JoinGameReq
{
    [RegularExpression(@"^\d{6}$", ErrorMessage = "The PIN must be exactly 6 digits.")]
    public int GamePin { get; set; } = 000000;
    public string PlayerName { get; set; } = null!;
}