namespace Domain.model;

public class Item
{
    public int Id { get; set; }
    public string Character { get; set; } = string.Empty;
    public CharacterType CharacterType { get; set; }
    public string? Meaning { get; set; }
    public int? JlptLevel { get; set; }

    public int BeltId { get; set; }
    public Belt? Belt { get; set; }
}

public enum CharacterType
{
    Hiragana = 1,
    Katakana = 2,
    Kanji = 3
}