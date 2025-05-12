using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DLL.Entities;

[Table("Players")]
public class Player
{
    [Key]
    public int Id { get; private init; }
    
    [Required]
    [MaxLength(50)]
    public string Name { get; private set; } = null!;
    
    [Range(0, int.MaxValue)]
    public int Score { get; private set; }
    
    [ForeignKey("Game")]
    public int GameId { get; private set; }
    [JsonIgnore]
    public Game Game { get; private set; } = null!;
    
    [Required]
    public bool IsHost { get; private set; }
    
    public ICollection<Drink> Drinks { get; private set; } = new HashSet<Drink>();
    
    private Player() { }
    
    public Player(string name, Game game, bool isHost = false)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be null or empty", nameof(name));
        
        Name = name;
        Game = game ?? throw new ArgumentNullException(nameof(game));
        GameId = game.Id;
        IsHost = isHost;
        Score = 0;
    }
    
    //add optional methods for addscore, rename etc.
}