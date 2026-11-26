using Domain.model;

namespace Core;

public static class CardFactory
{
    public static List<Card> CreateCards(int userId, IEnumerable<Item> items, DateTime now)
    {
        return items
            .SelectMany(item => Enum.GetValues<CardType>()
                .Where(type => type != CardType.Meaning || item.CharacterType == CharacterType.Kanji)
                .Select(type => new Card
                {
                    UserId = userId,
                    ItemId = item.Id,
                    CardType = type,
                    EaseFactor = 2.5,
                    IntervalDays = 0,
                    Repetitions = 0,
                    DueAt = now
                }))
            .ToList();
    }
}
