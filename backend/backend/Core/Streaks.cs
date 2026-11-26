namespace Core;

public static class Streaks
{
    // a streak is still alive if you haven't reviewed yet today but did yesterday
    public static int Current(IEnumerable<DateOnly> reviewDays, DateOnly today)
    {
        var days = reviewDays.ToHashSet();
        var day = days.Contains(today) ? today : today.AddDays(-1);
        var streak = 0;

        while (days.Contains(day))
        {
            streak++;
            day = day.AddDays(-1);
        }

        return streak;
    }

    public static int Longest(IEnumerable<DateOnly> reviewDays)
    {
        var longest = 0;
        var streak = 0;
        DateOnly? previous = null;

        foreach (var day in reviewDays.Distinct().Order())
        {
            streak = previous?.AddDays(1) == day ? streak + 1 : 1;
            longest = Math.Max(longest, streak);
            previous = day;
        }

        return longest;
    }
}
