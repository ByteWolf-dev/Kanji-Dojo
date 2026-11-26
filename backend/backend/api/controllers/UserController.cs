using api.Dtos;
using api.Dtos.model;
using Domain.model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    public UserController(KanjiDbContext context, IGenericMapper<UserDto, User> userMapper)
    {
        _kanjiDbContext = context;
        _userMapper = userMapper;
    }

    private KanjiDbContext _kanjiDbContext { get; }
    private IGenericMapper<UserDto, User> _userMapper { get; }

    [HttpGet]
    public List<UserDto> GetAllUsers()
    {
        return _kanjiDbContext.Users.ToList().Select(u => _userMapper.ToDto(u)).ToList();
    }
}