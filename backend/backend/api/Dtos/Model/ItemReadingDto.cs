using Domain.model;

namespace api.Dtos.model;

public class ItemReadingDto
{
    public int Id { get; set; }
    public int ItemId { get; set; }
    public string ReadingKana { get; set; }
    public string ReadingRomaji { get; set; }
    public Pronunciation Pronunciation { get; set; }
}