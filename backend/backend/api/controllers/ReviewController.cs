using api.Dtos;
using api.Dtos.model;
using Core;
using Domain.model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.controllers;

[Authorize]
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

    [HttpPost]
    public ActionResult<ReviewDto> ReviewCard([FromBody] ReviewDto reviewDto)
    {
        try
        {
            Review dbReview = _reviewMapper.ToEntity(reviewDto, _kanjiDbContext);
            Card? dbCard = _kanjiDbContext.Cards.FirstOrDefault(c => c.Id == dbReview.CardId);

            if (dbCard == null) return BadRequest("The card which the review was performed on does not exist.");

            dbReview.Card = dbCard;
        
            SchedulingAlgorithms.Review(dbCard, dbReview.Grade);
            _kanjiDbContext.SaveChanges();
            return Ok(dbReview);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}