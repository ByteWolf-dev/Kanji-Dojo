using api.Dtos;
using api.Dtos.Mappers;
using api.Dtos.model;
using Domain.model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private KanjiDbContext _kanjiDbContext { get; set; }
    private IGenericMapper<UserDto, User> _userMapper { get; set; }

    public UserController(KanjiDbContext context, IGenericMapper<UserDto, User> userMapper)
    {
        _kanjiDbContext = context;
        _userMapper = userMapper;
    }

    [HttpGet]
    public List<UserDto> GetAllUsers()
    {
        return _kanjiDbContext.Users.ToList().Select(u => _userMapper.ToDto(u)).ToList();
    }
}