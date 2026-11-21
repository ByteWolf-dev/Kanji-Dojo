using api.Dtos;
using api.Dtos.model;
using Domain.model;
using Microsoft.AspNetCore.Mvc;

namespace api;

[ApiController]
[Route("[controller]")]
public class ItemController : ControllerBase
{
    private KanjiDbContext _kanjiDbContext { get; set; }
    private IGenericMapper<ItemDto, Item> _itemMapper { get; set; }

    public ItemController(KanjiDbContext kanjiDbContext, IGenericMapper<ItemDto, Item> itemMapper)
    {
        _kanjiDbContext = kanjiDbContext;
        _itemMapper = itemMapper;
    }

    [HttpGet]
    public List<ItemDto> Get()
    {
        return _kanjiDbContext.Items.ToList().Select(_itemMapper.ToDto).ToList();
    }
}