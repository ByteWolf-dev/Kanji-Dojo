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
public class DojoController : ControllerBase
{
    public DojoController(KanjiDbContext kanjiDbContext)
    {
        _kanjiDbContext = kanjiDbContext;
    }

    private KanjiDbContext _kanjiDbContext { get; }

    [HttpGet("progress")]
    public async Task<ActionResult<ProgressDto>> GetProgress()
    {
        var user = await GetCurrentUser();

        if (user == null) return Unauthorized();

        var now = DateTime.UtcNow;
        var nextBelt = await GetNextBelt(user.Belt);

        var reviewDays = await _kanjiDbContext.Reviews
            .Where(r => r.Card.UserId == user.Id)
            .Select(r => r.ReviewedAt.Date)
            .Distinct()
            .ToListAsync();

        var days = reviewDays.Select(DateOnly.FromDateTime).ToList();
        var score = await GetScore(user.Id);

        return Ok(new ProgressDto
        {
            BeltName = user.Belt.Name,
            BeltColor = user.Belt.ColorHex,
            Rank = user.Belt.Rank,
            Score = score,
            NextBeltName = nextBelt?.Name,
            NextBeltColor = nextBelt?.ColorHex,
            NextBeltRequiredScore = nextBelt?.RequiredScore,
            CanFightBoss = nextBelt != null && score >= nextBelt.RequiredScore,
            CurrentStreak = Streaks.Current(days, DateOnly.FromDateTime(now)),
            LongestStreak = Streaks.Longest(days),
            ReviewedToday = await _kanjiDbContext.Reviews
                .CountAsync(r => r.Card.UserId == user.Id && r.ReviewedAt >= now.Date),
            DueCount = await _kanjiDbContext.Cards
                .CountAsync(c => c.UserId == user.Id && c.DueAt <= now)
        });
    }

    [HttpGet("boss")]
    public async Task<ActionResult<List<BossQuestionDto>>> GetBossFight()
    {
        var user = await GetCurrentUser();

        if (user == null) return Unauthorized();

        var error = await CheckCanFightBoss(user);

        if (error != null) return BadRequest(error);

        var questions = await _kanjiDbContext.Items
            .Where(i => i.BeltId == user.BeltId)
            .OrderBy(_ => EF.Functions.Random())
            .Take(BossFight.QuestionCount)
            .Select(i => new BossQuestionDto
            {
                ItemId = i.Id,
                Character = i.Character,
                CharacterType = i.CharacterType
            })
            .ToListAsync();

        return Ok(questions);
    }

    [HttpPost("boss")]
    public async Task<ActionResult<BossFightResult>> SubmitBossFight([FromBody] List<BossAnswerDto> answers)
    {
        var user = await GetCurrentUser();

        if (user == null) return Unauthorized();

        var error = await CheckCanFightBoss(user);

        if (error != null) return BadRequest(error);

        var itemIds = answers.Select(a => a.ItemId).Distinct().ToList();

        if (answers.Count != BossFight.QuestionCount || itemIds.Count != answers.Count)
            return BadRequest($"The boss fight needs {BossFight.QuestionCount} different answers.");

        var readings = await _kanjiDbContext.ItemReadings
            .Where(r => itemIds.Contains(r.ItemId) && r.Item.BeltId == user.BeltId)
            .ToListAsync();

        var readingsByItem = readings
            .GroupBy(r => r.ItemId)
            .ToDictionary(
                g => g.Key,
                g => g.SelectMany(r => new[] { r.ReadingKana, r.ReadingRomaji }).ToList());

        var result = BossFight.Grade(
            answers.Select(a => new BossFightAnswer(a.ItemId, a.Answer ?? string.Empty)).ToList(),
            readingsByItem);

        if (result.Passed)
        {
            var nextBelt = (await GetNextBelt(user.Belt))!;
            var newItems = await _kanjiDbContext.Items
                .Where(i => i.BeltId == nextBelt.Id)
                .ToListAsync();

            user.BeltId = nextBelt.Id;
            _kanjiDbContext.Cards.AddRange(CardFactory.CreateCards(user.Id, newItems, DateTime.UtcNow));

            await _kanjiDbContext.SaveChangesAsync();
        }

        return Ok(result);
    }

    private async Task<User?> GetCurrentUser()
    {
        var userId = User.GetUserId();

        if (userId == null) return null;

        return await _kanjiDbContext.Users
            .Include(u => u.Belt)
            .FirstOrDefaultAsync(u => u.Id == userId);
    }

    private Task<Belt?> GetNextBelt(Belt belt)
    {
        return _kanjiDbContext.Belts
            .Where(b => b.Rank > belt.Rank)
            .OrderBy(b => b.Rank)
            .FirstOrDefaultAsync();
    }

    private Task<int> GetScore(int userId)
    {
        return _kanjiDbContext.Reviews
            .CountAsync(r => r.Card.UserId == userId && r.IsCorrect);
    }

    private async Task<string?> CheckCanFightBoss(User user)
    {
        var nextBelt = await GetNextBelt(user.Belt);

        if (nextBelt == null) return "You already have the highest belt.";

        if (await GetScore(user.Id) < nextBelt.RequiredScore)
            return $"You need a score of {nextBelt.RequiredScore} to fight the boss.";

        return null;
    }
}
