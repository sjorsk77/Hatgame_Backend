using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DLL.Entities;

[Table("Rules")]
public class Rule
{
    [Key]
    public int Id { get; init; }
    
    [Required]
    [MaxLength(500)]
    public string Description { get; set; } = null!;
}