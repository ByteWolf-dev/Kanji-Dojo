using Core;
using Domain.model;

namespace Tests;

public class BossFightTests
{
    private static readonly Dictionary<int, List<string>> Readings = new()
    {
        [1] = ["あ", "a"],
        [2] = ["みず", "mizu", "スイ", "sui"]
    };

    private static List<BossFightAnswer> Answers(int correct, int total)
    {
        return Enumerable.Range(0, total)
            .Select(i => new BossFightAnswer(1, i < correct ? "あ" : "い"))
            .ToList();
    }

    [Fact]
    public void EightOutOfTen_Passes()
    {
        var result = BossFight.Grade(Answers(8, 10), Readings);

        Assert.Equal(new BossFightResult(8, 10, true), result);
    }

    [Fact]
    public void SevenOutOfTen_Fails()
    {
        Assert.False(BossFight.Grade(Answers(7, 10), Readings).Passed);
    }

    [Fact]
    public void NoAnswers_Fails()
    {
        Assert.False(BossFight.Grade([], Readings).Passed);
    }

    [Theory]
    [InlineData("みず")]
    [InlineData("すい")]
    [InlineData("スイ")]
    [InlineData("MIZU")]
    [InlineData(" mizu ")]
    public void AnyReading_InKanaOrRomaji_IsCorrect(string answer)
    {
        var result = BossFight.Grade([new BossFightAnswer(2, answer)], Readings);

        Assert.Equal(1, result.Correct);
    }

    [Fact]
    public void UnknownItem_IsWrong()
    {
        var result = BossFight.Grade([new BossFightAnswer(99, "あ")], Readings);

        Assert.Equal(0, result.Correct);
    }
}

public class CardFactoryTests
{
    [Fact]
    public void Kana_GetsRecognitionAndWritingCards()
    {
        var item = new Item { Id = 1, CharacterType = CharacterType.Hiragana };

        var cards = CardFactory.CreateCards(5, [item], DateTime.UtcNow);

        Assert.Equal([CardType.Recognition, CardType.Writing], cards.Select(c => c.CardType));
        Assert.All(cards, c => Assert.Equal(5, c.UserId));
    }

    [Fact]
    public void Kanji_AlsoGetsAMeaningCard()
    {
        var item = new Item { Id = 1, CharacterType = CharacterType.Kanji };

        var cards = CardFactory.CreateCards(5, [item], DateTime.UtcNow);

        Assert.Equal(3, cards.Count);
        Assert.Contains(cards, c => c.CardType == CardType.Meaning);
    }

    [Fact]
    public void NewCards_StartWithDefaultEaseAndAreDueNow()
    {
        var now = DateTime.UtcNow;

        var card = CardFactory.CreateCards(5, [new Item { Id = 1 }], now)[0];

        Assert.Equal(2.5, card.EaseFactor);
        Assert.Equal(0, card.Repetitions);
        Assert.Equal(now, card.DueAt);
    }
}
