using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DLL.Entities;

[Table("Players")]
public class Player
{
    [Key]
    public int Id { get; init; }
    
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = null!;
    
    [Range(0, int.MaxValue)]
    public int Score { get; set; } = 0;
    
    [ForeignKey("Game")]
    public int GameId { get; set; }
    [JsonIgnore]
    public Game Game { get; set; } = null!;
    
    public ICollection<Drink> Drinks { get; set; } = new HashSet<Drink>();
}