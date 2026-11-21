using Domain.model;

namespace api.Dtos.model;

public class ItemDto
{
    public int Id { get; set; }
    public string Character { get; set; } =  string.Empty;
    public CharacterType  CharacterType { get; set; }
    public string? Meaning { get; set; }
    public int? JlptLevel { get; set; }
    
    public int BeltId { get; set; }
}