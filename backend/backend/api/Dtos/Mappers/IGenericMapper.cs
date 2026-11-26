namespace api.Dtos;

public interface IGenericMapper<TDto, TEntity>
{
    public TDto ToDto(TEntity entity);
    public TEntity ToEntity(TDto dto, KanjiDbContext context);
}

// use mapster in the next project, writing this by hand is a pain.