using api.Dtos.model;
using Domain.model;

namespace api.Dtos.Mappers;

public class ReviewMapper : IGenericMapper<ReviewDto, Review>
{
    public ReviewDto ToDto(Review entity)
    {
        return new ReviewDto()
        {
            Id = entity.Id,
            Answer = entity.Answer,
            CardId = entity.CardId,
            Grade = entity.Grade,
            IsCorrect = entity.IsCorrect,
            ReviewedAt = entity.ReviewedAt,
        };
    }

    public Review ToEntity(ReviewDto dto, KanjiDbContext db)
    {
        return db.Reviews.FirstOrDefault(x => x.Id == dto.Id) ?? new Review()
        {
            Id = dto.Id,
            Answer = dto.Answer,
            CardId = dto.CardId,
            Grade = dto.Grade,
            IsCorrect = dto.IsCorrect,
            ReviewedAt = dto.ReviewedAt,
        };
    }
}