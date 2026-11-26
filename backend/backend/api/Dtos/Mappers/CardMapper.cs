using api.Dtos.model;
using Domain.model;

namespace api.Dtos.Mappers;

public class CardMapper : IGenericMapper<CardDto, Card>
{
    public CardDto ToDto(Card entity)
    {
        return new CardDto
        {
            Id = entity.Id,
            CardType = entity.CardType,
            DueAt = entity.DueAt,
            EaseFactor = entity.EaseFactor,
            IntervalDays = entity.IntervalDays,
            ItemId = entity.ItemId,
            LastReviewAt = entity.LastReviewAt,
            UserId = entity.UserId
        };
    }

    public Card ToEntity(CardDto dto, KanjiDbContext db)
    {
        return db.Cards.FirstOrDefault(x => x.Id == dto.Id) ?? new Card
        {
            Id = dto.Id,
            CardType = dto.CardType,
            DueAt = dto.DueAt,
            EaseFactor = dto.EaseFactor,
            IntervalDays = dto.IntervalDays,
            ItemId = dto.ItemId,
            LastReviewAt = dto.LastReviewAt,
            UserId = dto.UserId
        };
    }
}