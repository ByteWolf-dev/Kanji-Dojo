using Domain.model;

namespace api.Dtos.model;

public class CardDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int ItemId { get; set; }
    public CardType CardType { get; set; }
    public double EaseFactor { get; set; }
    public int IntervalDays { get; set; }
    public int Repetitions { get; set; }
    public DateTime DueAt { get; set; }
    public DateTime LastReviewAt { get; set; }
}