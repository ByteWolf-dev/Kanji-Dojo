using Domain.model;

namespace api.Dtos.model;

public class BossQuestionDto
{
    public int ItemId { get; set; }
    public string Character { get; set; } = string.Empty;
    public CharacterType CharacterType { get; set; }
}
