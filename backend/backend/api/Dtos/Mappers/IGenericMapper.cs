using Microsoft.EntityFrameworkCore;

namespace api.Dtos;

public interface IGenericMapper<TDto,TEntity>
{
    public TDto ToDto(TEntity entity);
    public TEntity ToEntity(TDto dto, KanjiDbContext context);
}