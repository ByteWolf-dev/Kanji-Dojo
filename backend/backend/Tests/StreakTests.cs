using Core;

namespace Tests;

public class StreakTests
{
    private static readonly DateOnly Today = new(2026, 11, 25);

    private static IEnumerable<DateOnly> DaysAgo(params int[] days)
    {
        return days.Select(d => Today.AddDays(-d));
    }

    [Fact]
    public void NoReviews_StreakIsZero()
    {
        Assert.Equal(0, Streaks.Current([], Today));
        Assert.Equal(0, Streaks.Longest([]));
    }

    [Fact]
    public void ConsecutiveDaysUntilToday_AreCounted()
    {
        Assert.Equal(3, Streaks.Current(DaysAgo(0, 1, 2), Today));
    }

    [Fact]
    public void NoReviewToday_StreakFromYesterdayIsStillAlive()
    {
        Assert.Equal(2, Streaks.Current(DaysAgo(1, 2), Today));
    }

    [Fact]
    public void MissedDay_BreaksTheStreak()
    {
        Assert.Equal(1, Streaks.Current(DaysAgo(0, 2, 3, 4), Today));
        Assert.Equal(0, Streaks.Current(DaysAgo(2, 3), Today));
    }

    [Fact]
    public void DuplicateDays_AreOnlyCountedOnce()
    {
        Assert.Equal(2, Streaks.Current(DaysAgo(0, 0, 1, 1), Today));
        Assert.Equal(2, Streaks.Longest(DaysAgo(0, 0, 1, 1)));
    }

    [Fact]
    public void Longest_FindsTheLongestRun()
    {
        Assert.Equal(4, Streaks.Longest(DaysAgo(0, 1, 5, 6, 7, 8, 10)));
    }
}
