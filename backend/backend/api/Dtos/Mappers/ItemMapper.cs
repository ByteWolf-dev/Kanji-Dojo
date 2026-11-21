using api.Dtos.model;
using Domain.model;

namespace api.Dtos.Mappers;

public class ItemMapper : IGenericMapper<ItemDto, Item>
{
    public ItemDto ToDto(Item entity)
    {
        return new ItemDto
        {
            Id = entity.Id,
            Character = entity.Character,
            CharacterType = entity.CharacterType,
            Meaning = entity.Meaning,
            JlptLevel = entity.JlptLevel,
            BeltId = entity.BeltId,
        };
    }

    public Item ToEntity(ItemDto dto, KanjiDbContext db)
    {
        return db.Items.FirstOrDefault(x => x.Id == dto.Id) ?? new Item()
        {
            Id = dto.Id,
            Character = dto.Character,
            CharacterType = dto.CharacterType,
            Meaning = dto.Meaning,
            JlptLevel = dto.JlptLevel,
            BeltId = dto.BeltId,
        };
    }
}