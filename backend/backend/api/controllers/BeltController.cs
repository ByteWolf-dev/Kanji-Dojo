using Domain.model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class BeltController : ControllerBase
{
    public BeltController(KanjiDbContext kanjiDbContext)
    {
        _kanjiDbContext = kanjiDbContext;
    }

    private KanjiDbContext _kanjiDbContext { get; }

    [HttpGet]
    public List<Belt> Get()
    {
        return _kanjiDbContext.Belts.ToList();
    }
}