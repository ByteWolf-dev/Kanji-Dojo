namespace Domain.model;

public class ItemReading
{
    public int Id { get; set; }
    
    public int ItemId { get; set; }
    public Item Item { get; set; }
    
    public string ReadingKana  { get; set; }
    public string ReadingRomaji { get; set; }
    public Pronunciation Pronunciation { get; set; }
}

public enum Pronunciation
{
    Standard,
    OnYomi,
    KunYomi
}