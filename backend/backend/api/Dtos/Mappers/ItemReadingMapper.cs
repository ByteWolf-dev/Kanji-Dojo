using api.Dtos.model;
using Domain.model;

namespace api.Dtos.Mappers;

public class ItemReadingMapper : IGenericMapper<ItemReadingDto, ItemReading>
{
    public ItemReadingDto ToDto(ItemReading entity)
    {
        return new ItemReadingDto
        {
            Id = entity.Id,
            ItemId = entity.ItemId,
            Pronunciation = entity.Pronunciation,
            ReadingKana = entity.ReadingKana,
            ReadingRomaji = entity.ReadingRomaji
        };
    }

    public ItemReading ToEntity(ItemReadingDto dto, KanjiDbContext db)
    {
        return db.ItemReadings.FirstOrDefault(x => x.Id == dto.ItemId) ?? new ItemReading
        {
            Id = dto.ItemId,
            ItemId = dto.ItemId,
            Pronunciation = dto.Pronunciation,
            ReadingKana = dto.ReadingKana,
            ReadingRomaji = dto.ReadingRomaji
        };
    }
}