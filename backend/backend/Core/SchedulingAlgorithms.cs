using Domain.model;

namespace Core;

public class SchedulingAlgorithms
{
    public static void Review(Card card, int quality)
    {
        if (quality < 0 || quality > 5)
            throw new ArgumentOutOfRangeException(nameof(quality));

        if (quality >= 3)
        {
            card.IntervalDays = card.Repetitions switch
            {
                0 => 1,
                1 => 6,
                _ => (int)Math.Ceiling(
                    card.IntervalDays * card.EaseFactor)
            };

            card.Repetitions++;
        }
        else
        {
            card.Repetitions = 0;
            card.IntervalDays = 1;
        }

        card.EaseFactor +=
            0.1 - (5 - quality) *
            (0.08 + (5 - quality) * 0.02);

        card.EaseFactor = Math.Max(1.3, card.EaseFactor);

        card.DueAt = DateTime.UtcNow
            .AddDays(card.IntervalDays);
    }
}