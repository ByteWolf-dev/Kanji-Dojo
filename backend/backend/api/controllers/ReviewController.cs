using api.Dtos;
using api.Dtos.model;
using api.Extensions;
using Core;
using Domain.model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ReviewController : ControllerBase
{
    public ReviewController(KanjiDbContext kanjiDbContext, IGenericMapper<ReviewDto, Review> reviewMapper)
    {
        _kanjiDbContext = kanjiDbContext;
        _reviewMapper = reviewMapper;
    }

    private KanjiDbContext _kanjiDbContext { get; }
    private IGenericMapper<ReviewDto, Review> _reviewMapper { get; }

    [HttpGet]
    public async Task<ActionResult<List<ReviewDto>>> GetMyReviews()
    {
        var userId = User.GetUserId();

        if (userId == null) return Unauthorized();

        var reviews = await _kanjiDbContext.Reviews
            .AsNoTracking()
            .Where(r => r.Card.UserId == userId)
            .OrderByDescending(r => r.ReviewedAt)
            .ToListAsync();

        return Ok(reviews.Select(_reviewMapper.ToDto).ToList());
    }

    [HttpPost]
    public async Task<ActionResult<ReviewDto>> ReviewCard([FromBody] ReviewDto reviewDto)
    {
        var userId = User.GetUserId();

        if (userId == null) return Unauthorized();

        if (reviewDto.Grade < 0 || reviewDto.Grade > 5) return BadRequest("Grade has to be between 0 and 5.");

        var dbCard = await _kanjiDbContext.Cards
            .FirstOrDefaultAsync(c => c.Id == reviewDto.CardId && c.UserId == userId);

        if (dbCard == null) return BadRequest("The card which the review was performed on does not exist.");

        var now = DateTime.UtcNow;

        var review = new Review
        {
            Card = dbCard,
            Answer = reviewDto.Answer ?? string.Empty,
            Grade = reviewDto.Grade,
            IsCorrect = reviewDto.Grade >= 3,
            ReviewedAt = now
        };

        SchedulingAlgorithms.Review(dbCard, review.Grade, now);

        _kanjiDbContext.Reviews.Add(review);
        await _kanjiDbContext.SaveChangesAsync();

        return Ok(_reviewMapper.ToDto(review));
    }
}
