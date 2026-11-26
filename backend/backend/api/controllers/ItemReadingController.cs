using api.Dtos;
using api.Dtos.model;
using Domain.model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ItemReadingController : ControllerBase
{
    public ItemReadingController(KanjiDbContext kanjiDbContext, IGenericMapper<ItemReadingDto, ItemReading> cardMapper)
    {
        _kanjiDbContext = kanjiDbContext;
        _cardMapper = cardMapper;
    }

    private KanjiDbContext _kanjiDbContext { get; }
    private IGenericMapper<ItemReadingDto, ItemReading> _cardMapper { get; }

    [HttpGet]
    public List<ItemReadingDto> Get()
    {
        return _kanjiDbContext.ItemReadings.Select(_cardMapper.ToDto).ToList();
    }
}