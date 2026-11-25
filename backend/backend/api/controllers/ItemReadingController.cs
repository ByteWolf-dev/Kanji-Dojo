using api.Dtos;
using api.Dtos.model;
using Domain.model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class ItemReadingController : ControllerBase
{
    private KanjiDbContext _kanjiDbContext { get; set; }
    private IGenericMapper<ItemReadingDto, ItemReading> _cardMapper { get; set; }

    public ItemReadingController(KanjiDbContext kanjiDbContext, IGenericMapper<ItemReadingDto, ItemReading> cardMapper)
    {
        _kanjiDbContext = kanjiDbContext;
        _cardMapper = cardMapper;
    }

    [HttpGet]
    public List<ItemReadingDto> Get()
    {
        return _kanjiDbContext.ItemReadings.Select(_cardMapper.ToDto).ToList();
    }
}