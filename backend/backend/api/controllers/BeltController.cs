using api.Dtos;
using Domain.model;
using Microsoft.AspNetCore.Mvc;

namespace api;

[ApiController]
[Route("[controller]")]
public class BeltController : ControllerBase
{
    private KanjiDbContext _kanjiDbContext { get; set; }

    public BeltController(KanjiDbContext kanjiDbContext)
    {
        _kanjiDbContext = kanjiDbContext;
    }
    
    [HttpGet]
    public List<Belt> Get()
    {
        return _kanjiDbContext.Belts.ToList();
    }
}