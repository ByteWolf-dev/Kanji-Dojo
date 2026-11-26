using api.Dtos.model;
using Domain.model;

namespace api.Dtos.Mappers;

public class UserMapper : IGenericMapper<UserDto, User>
{
    public UserDto ToDto(User entity)
    {
        return new UserDto
        {
            UserId = entity.Id,
            Name = entity.Name,
            BeltId = entity.BeltId
        };
    }

    public User ToEntity(UserDto dto, KanjiDbContext db)
    {
        return db.Users.FirstOrDefault(u => u.Id == dto.UserId)
               ?? new User
               {
                   Name = dto.Name,
                   BeltId = dto.BeltId
               };
    }
}