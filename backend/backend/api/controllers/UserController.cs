using api.Dtos;
using api.Dtos.model;
using Domain.model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.controllers;

[Authorize]
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