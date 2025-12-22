using BookingAPI.Data.Dtos;
using BookingAPI.Data.Mappers;
using BookingAPI.Data.Repositories;
using BookingAPI.Extensions;
using BookingAPI.Services;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace BookingAPI.Controllers;

[ApiController]
[Route("api/desks/{deskId}/reservations")]
public class ReservationController(IReservationsRepository reservationsRepository, IReservationService reservationService, IDesksRepository desksRepository, IUsersRepository usersRepository, 
    IValidator<CreateReservationDto> createValidator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ReservationDto>>> GetActiveReservations(int deskId)
    {
        var desk = await desksRepository.GetByIdAsync(deskId);
        if (desk == null)
            return NotFound("Desk not found.");
        
        var reservations = await reservationsRepository.GetActiveReservationsForDeskAsync(deskId);
        var reservationsDto = reservations.Select(g => g.ToReservationDto());

        return Ok(reservationsDto);
    }
    
    [HttpPost]
    [Route("{reservationId}/cancel")]
    public async Task<IActionResult> CancelReservation(int deskId, int reservationId, [FromBody] CancelReservationDto? request)
    {
        var reservation = await reservationsRepository.GetByIdAsync(reservationId);
        if (reservation == null)
            return NotFound("Reservation not found.");
        
        if (reservation.DeskId != deskId)
            return BadRequest("Reservation does not belong to the specified desk.");

        try
        {
            await reservationService.CancelReservationAsync(reservation, request?.Date);
        }
        catch (InvalidOperationException ex) 
        {
            return BadRequest(ex.Message);
        }
            
        return Ok(new { message  = "Reservation updated successfully." });
    }

    [HttpPost]
    public async Task<IActionResult> PostReservation(int deskId, [FromBody] CreateReservationDto createReservationDto)
    {
        ValidationResult validationResult = await createValidator.ValidateAsync(createReservationDto);
        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);
            return BadRequest(ModelState);
        }
        
        var desk = await desksRepository.GetByIdAsync(deskId);
        if (desk == null)
            return NotFound("Desk not found.");
        
        var user = await usersRepository.GetByIdAsync(createReservationDto.UserId);
        if (user == null)
            return NotFound("User not found.");
        
        try
        {
            await reservationService.ReserveDeskAsync(desk, createReservationDto);
            return Created();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}