namespace Domain.model;

public class Card
{
    public int Id { get; set; }
    
    public int UserId { get; set; }
    public User User { get; set; }
    
    public int ItemId { get; set; }
    public Item Item { get; set; }
    
    
    public CardType CardType { get; set; }
    public double EaseFactor { get; set; }
    public int IntervalDays { get; set; }
    public int Repetitions { get; set; }
    public DateTime DueAt { get; set; }
    public DateTime LastReviewAt { get; set; }
}

public enum CardType
{
    Recognition,
    Writing,
    Meaning
}