using api.Dtos;
using api.Dtos.model;
using Domain.model;
using Microsoft.AspNetCore.Mvc;

namespace api;

[ApiController]
[Route("[controller]")]
public class ReviewController : ControllerBase
{
    private KanjiDbContext _kanjiDbContext { get; set; }
    private IGenericMapper<ReviewDto, Review> _reviewMapper { get; set; }

    public ReviewController(KanjiDbContext kanjiDbContext, IGenericMapper<ReviewDto, Review> reviewMapper)
    {
        _kanjiDbContext = kanjiDbContext;
        _reviewMapper = reviewMapper;
    }

    [HttpGet]
    public List<ReviewDto> Get()
    {
        return _kanjiDbContext.Reviews.Select(_reviewMapper.ToDto).ToList();
    }
}