namespace api.Dtos.model;

using System.ComponentModel.DataAnnotations;

public class LoginDto
{
    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}