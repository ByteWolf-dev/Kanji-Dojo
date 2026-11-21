using api.Dtos;
using api.Dtos.model;
using Domain.model;
using Microsoft.AspNetCore.Mvc;

namespace api;

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
}