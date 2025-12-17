using BookingAPI.Data.Dtos;
using BookingAPI.Data.Mappers;
using BookingAPI.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BookingAPI.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController(IUsersRepository usersRepository) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
    {
        var users = await usersRepository.GetAllAsync();
        var usersDto = users.Select(g => g.ToUserDto());

        return Ok(usersDto);
    }
    
    [HttpGet]
    [Route("{userId}")]
    public async Task<ActionResult<UserDto>> GetUser(int userId)
    {
        var user = await usersRepository.GetByIdAsync(userId);
        if (user == null)
            return NotFound();
        
        return Ok(user.ToUserDto());
    }
}