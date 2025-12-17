using BookingAPI.Data.Dtos;
using BookingAPI.Data.Mappers;
using BookingAPI.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BookingAPI.Controllers;

[ApiController]
[Route("api/reservations")]
public class ReservationController(IReservationsRepository reservationsRepository) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ReservationDto>>> GetReservations()
    {
        var reservations = await reservationsRepository.GetAllAsync();
        var reservationsDto = reservations.Select(g => g.ToReservationDto());

        return Ok(reservationsDto);
    }
    
    [HttpGet]
    [Route("{reservationId}")]
    public async Task<ActionResult<ReservationDto>> GetReservation(int reservationId)
    {
        var reservation = await reservationsRepository.GetByIdAsync(reservationId);
        if (reservation == null)
            return NotFound();
        
        return Ok(reservation.ToReservationDto());
    }
}