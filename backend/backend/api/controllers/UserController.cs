using Domain.model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private KanjiDbContext _kanjiDbContext { get; set; }

    public UserController(KanjiDbContext context)
    {
        _kanjiDbContext = context;
    }

    [HttpGet]
    public IEnumerable<User> GetAllUsers()
    {
        return _kanjiDbContext.Users.ToList();
    }
}