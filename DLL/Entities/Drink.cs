using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.Extensions.DependencyInjection;

namespace DLL.Entities;

[Table("Drinks")]
public class Drink
{
    [Key]
    public int Id { get; init; }
    
    [ForeignKey("Player")]
    public int PlayerId { get; set; }
    [JsonIgnore]
    public Player Player { get; set; } = null!;

    [ForeignKey("DrinkType")] public int DrinkTypeId { get; set; }
    public DrinkType DrinkType { get; set; } = null!;
}