using Core;
using Domain.model;

namespace Tests;

public class Algorithm
{
    private static readonly DateTime Now = new(2026, 11, 23, 10, 0, 0, DateTimeKind.Utc);

    private static Card NewCard()
    {
        return new Card
        {
            EaseFactor = 2.5,
            IntervalDays = 0,
            Repetitions = 0
        };
    }

    [Fact]
    public void FirstCorrectReview_IntervalIsOneDay()
    {
        var card = NewCard();

        SchedulingAlgorithms.Review(card, 4, Now);

        Assert.Equal(1, card.IntervalDays);
        Assert.Equal(1, card.Repetitions);
        Assert.Equal(Now.AddDays(1), card.DueAt);
        Assert.Equal(Now, card.LastReviewAt);
    }

    [Fact]
    public void SecondCorrectReview_IntervalIsSixDays()
    {
        var card = NewCard();

        SchedulingAlgorithms.Review(card, 4, Now);
        SchedulingAlgorithms.Review(card, 4, Now);

        Assert.Equal(6, card.IntervalDays);
        Assert.Equal(2, card.Repetitions);
    }

    [Fact]
    public void ThirdCorrectReview_IntervalIsMultipliedByEaseFactor()
    {
        var card = NewCard();

        SchedulingAlgorithms.Review(card, 4, Now);
        SchedulingAlgorithms.Review(card, 4, Now);
        SchedulingAlgorithms.Review(card, 4, Now);

        // 6 * 2.5 = 15
        Assert.Equal(15, card.IntervalDays);
        Assert.Equal(3, card.Repetitions);
    }

    [Theory]
    [InlineData(5, 2.6)]
    [InlineData(4, 2.5)]
    [InlineData(3, 2.36)]
    [InlineData(2, 2.18)]
    [InlineData(1, 1.96)]
    [InlineData(0, 1.7)]
    public void EaseFactor_ChangesBasedOnQuality(int quality, double expected)
    {
        var card = NewCard();

        SchedulingAlgorithms.Review(card, quality, Now);

        Assert.Equal(expected, card.EaseFactor, 5);
    }

    [Fact]
    public void EaseFactor_NeverDropsBelowMinimum()
    {
        var card = NewCard();

        for (var i = 0; i < 10; i++)
            SchedulingAlgorithms.Review(card, 0, Now);

        Assert.Equal(1.3, card.EaseFactor, 5);
    }

    [Fact]
    public void Lapse_ResetsRepetitionsAndInterval()
    {
        var card = new Card
        {
            EaseFactor = 2.5,
            IntervalDays = 15,
            Repetitions = 3
        };

        SchedulingAlgorithms.Review(card, 2, Now);

        Assert.Equal(0, card.Repetitions);
        Assert.Equal(1, card.IntervalDays);
        Assert.Equal(Now.AddDays(1), card.DueAt);
    }

    [Fact]
    public void Lapse_KeepsLowerEaseFactorForNextIntervals()
    {
        var card = NewCard();

        SchedulingAlgorithms.Review(card, 1, Now);
        SchedulingAlgorithms.Review(card, 4, Now);
        SchedulingAlgorithms.Review(card, 4, Now);
        SchedulingAlgorithms.Review(card, 4, Now);

        // ease factor stays at 1.96 after the lapse, so 6 * 1.96 = 11.76 -> 12
        Assert.Equal(12, card.IntervalDays);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(6)]
    public void InvalidQuality_Throws(int quality)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            SchedulingAlgorithms.Review(NewCard(), quality, Now));
    }
}
