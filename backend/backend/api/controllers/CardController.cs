using api.Dtos;
using api.Dtos.model;
using Domain.model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class CardController : ControllerBase
{
    private KanjiDbContext _kanjiDbContext { get; set; }
    private IGenericMapper<CardDto, Card> _cardMapper { get; set; }
    
    public CardController(KanjiDbContext kanjiDbContext, IGenericMapper<CardDto, Card> cardMapper)
    {
        _kanjiDbContext = kanjiDbContext;
        _cardMapper = cardMapper;
    }

    [HttpGet]
    public List<CardDto> GetByUserId(int userId)
    {
        return _kanjiDbContext.Cards.Where(c => c.UserId == userId).ToList().Select(_cardMapper.ToDto).ToList();
    }

    [HttpGet("due")]
    public async Task<ActionResult<List<CardDto>>>
        GetCardsDueOnOrBefore([FromQuery] DateTime date)
    {
        var cards = await _kanjiDbContext.Cards
            .AsNoTracking()
            .Where(c => c.DueAt <= date)
            .OrderBy(c => c.DueAt)
            .ToListAsync();

        var cardDtos = cards
            .Select(c => _cardMapper.ToDto(c))
            .ToList();

        return Ok(cardDtos);
    }
}