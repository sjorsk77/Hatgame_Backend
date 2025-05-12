using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DLL.Entities;

[Table("Games")]
public class Game
{
    [Key]
    public int Id { get; init; }
    
    [Required]
    [MaxLength(6)]
    [MinLength(6)]
    public int Pin { get; set; } = 000000;

    [Required] 
    public bool IsLive { get; set; } = true;
    [Required]
    public string HubGroup { get; set; }

    public ICollection<Player> Players { get; set; } = new HashSet<Player>();
    
    private Game() { }
    
    public Game(string hubGroup, int pin, bool isLive = true)
    {
        if (string.IsNullOrWhiteSpace(hubGroup))
            throw new ArgumentException("HubGroup cannot be null or empty", nameof(hubGroup));
        
        HubGroup = hubGroup;
        Pin = pin;
        IsLive = isLive;
    }
}