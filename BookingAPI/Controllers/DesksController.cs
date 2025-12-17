using BookingAPI.Data.Dtos;
using BookingAPI.Data.Mappers;
using BookingAPI.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BookingAPI.Controllers;

[ApiController]
[Route("api/desks")]
public class DesksController(IDesksRepository desksRepository) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DeskDto>>> GetDesks()
    {
        var desks = await desksRepository.GetAllAsync();
        var desksDto = desks.Select(g => g.ToDeskDto());

        return Ok(desksDto);
    }
    
    [HttpGet]
    [Route("{deskId}")]
    public async Task<ActionResult<DeskDto>> GetDesk(int deskId)
    {
        var desk = await desksRepository.GetByIdAsync(deskId);
        if (desk == null)
            return NotFound();
        
        return Ok(desk.ToDeskDto());
    }
}