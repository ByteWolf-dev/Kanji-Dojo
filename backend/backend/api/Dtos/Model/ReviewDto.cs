namespace api.Dtos.model;

public class ReviewDto
{
    public int Id { get; set; }
    public int CardId { get; set; }
    public DateTime ReviewedAt { get; set; }
    public string? Answer { get; set; }
    public int Grade { get; set; }
    public bool IsCorrect { get; set; }
}