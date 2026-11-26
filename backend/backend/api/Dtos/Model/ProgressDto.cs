using Domain.model;

namespace api.Dtos.model;

public class ProgressDto
{
    public string BeltName { get; set; } = string.Empty;
    public string BeltColor { get; set; } = string.Empty;
    public Rank Rank { get; set; }
    public int Score { get; set; }

    public string? NextBeltName { get; set; }
    public string? NextBeltColor { get; set; }
    public double? NextBeltRequiredScore { get; set; }
    public bool CanFightBoss { get; set; }

    public int CurrentStreak { get; set; }
    public int LongestStreak { get; set; }
    public int ReviewedToday { get; set; }
    public int DueCount { get; set; }
}
