namespace Domain.model;

public class Review
{
    public int Id { get; set; }
    
    public int CardId { get; set; }
    public Card Card { get; set; }
    
    public DateTime ReviewedAt { get; set; }
    public string Answer { get; set; }
    public int Grade { get; set; }
    public bool IsCorrect { get; set; }
}