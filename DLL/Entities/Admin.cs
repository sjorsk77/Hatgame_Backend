using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DLL.Entities;

[Table("Admins")]
public class Admin
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(255)]
    public string Email { get; set; } = null!;
    
    [Required]
    [MaxLength(255)]
    public string Password { get; set; } = null!;
}