using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DLL.Entities;

[Table("Games")]
public class Game
{
    [Key]
    public int Id { get; init; }
    
    [Required]
    public string Name { get; set; } = null!;
    public string? Password { get; set; }
    
    [Required]
    public Player Host { get; set; } = null!;
    
    public ICollection<Rule> Rules { get; set; } = new HashSet<Rule>();
    public ICollection<Player> Players { get; set; } = new HashSet<Player>();
}