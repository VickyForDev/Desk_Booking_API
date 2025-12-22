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
    public async Task<ActionResult<IEnumerable<DeskDto>>> GetDesks([FromQuery] DateOnly? from, [FromQuery] DateOnly? to)
    {
        var desks = await desksRepository.GetAllByDateRangeAsync(from, to);
        var desksDto = desks.Select(g => g.ToDeskDto());

        return Ok(desksDto);
    }
}