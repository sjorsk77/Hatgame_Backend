using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DLL.Entities;

[Table("DrinkTypes")]
public class DrinkType
{
    [Key]
    public int Id { get; init; }
    
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = null!;
}