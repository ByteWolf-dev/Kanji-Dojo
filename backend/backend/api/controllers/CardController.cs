using api.Dtos;
using api.Dtos.model;
using api.Extensions;
using Domain.model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CardController : ControllerBase
{
    public CardController(KanjiDbContext kanjiDbContext, IGenericMapper<CardDto, Card> cardMapper)
    {
        _kanjiDbContext = kanjiDbContext;
        _cardMapper = cardMapper;
    }

    private KanjiDbContext _kanjiDbContext { get; }
    private IGenericMapper<CardDto, Card> _cardMapper { get; }

    [HttpGet]
    public async Task<ActionResult<List<CardDto>>> GetMyCards()
    {
        var userId = User.GetUserId();

        if (userId == null) return Unauthorized();

        var cards = await _kanjiDbContext.Cards
            .AsNoTracking()
            .Where(c => c.UserId == userId)
            .ToListAsync();

        return Ok(cards.Select(_cardMapper.ToDto).ToList());
    }

    [HttpGet("due")]
    public async Task<ActionResult<List<CardDto>>> GetCardsDueOnOrBefore([FromQuery] DateTime date)
    {
        var userId = User.GetUserId();

        if (userId == null) return Unauthorized();

        var cards = await _kanjiDbContext.Cards
            .AsNoTracking()
            .Where(c => c.UserId == userId && c.DueAt <= date)
            .OrderBy(c => c.DueAt)
            .ToListAsync();

        var cardDtos = cards
            .Select(c => _cardMapper.ToDto(c))
            .ToList();

        return Ok(cardDtos);
    }
}
