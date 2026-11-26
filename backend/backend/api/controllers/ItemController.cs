using api.Dtos;
using api.Dtos.model;
using Domain.model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ItemController : ControllerBase
{
    public ItemController(KanjiDbContext kanjiDbContext, IGenericMapper<ItemDto, Item> itemMapper)
    {
        _kanjiDbContext = kanjiDbContext;
        _itemMapper = itemMapper;
    }

    private KanjiDbContext _kanjiDbContext { get; }
    private IGenericMapper<ItemDto, Item> _itemMapper { get; }

    [HttpGet]
    public List<ItemDto> Get()
    {
        return _kanjiDbContext.Items.ToList().Select(_itemMapper.ToDto).ToList();
    }
}