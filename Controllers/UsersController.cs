namespace WebApi.Controllers;

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using WebApi.Models.Users;
using WebApi.Services;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private IUserService _userService;
    private IMapper _mapper;
    private static readonly Random random = new Random();

    public UsersController(
        IUserService userService,
        IMapper mapper)
    {
        _userService = userService;
        _mapper = mapper;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var users = _userService.GetAll();
        return Ok(users);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var user = _userService.GetById(id);
        return Ok(user);
    }
	
	[HttpGet("{username}")]
	public IActionResult GetUserByUsername(string username)
	{
		// Vulnerability: SQL Injection
		// Directly using user input in a SQL query without proper validation can lead to SQL injection.

		string query = $"SELECT * FROM Users WHERE Username = '{username}'";
		var user = _userService.ExecuteSqlQuery(query);

		return Ok(user);
	}

    [HttpPost]
    public IActionResult Create(CreateRequest model)
    {
        //20% of requests return an error
        if (random.NextDouble() < 0.2)
        {
            throw new ApplicationException("Oopss, Something gone wrong!");
        }
        _userService.Create(model);
        return Ok(new { message = "User created" });
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, UpdateRequest model)
    {
        _userService.Update(id, model);
        return Ok(new { message = "User updated" });
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        _userService.Delete(id);
        return Ok(new { message = "User deleted" });
    }
}