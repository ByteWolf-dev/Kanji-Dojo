using System.ComponentModel.DataAnnotations;

namespace api.Dtos.model;

public class RegisterDto
{
    [Required] [MaxLength(100)] public string Name { get; set; } = string.Empty;

    [Required]
    [MinLength(15)]
    [MaxLength(128)]
    public string Password { get; set; } = string.Empty;
}