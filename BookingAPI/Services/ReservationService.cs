using BookingAPI.Data.Dtos;
using BookingAPI.Data.Models;
using BookingAPI.Data.Models.Enums;
using BookingAPI.Data.Repositories;

namespace BookingAPI.Services;
public interface IReservationService
{
    Task CancelReservationAsync(Reservation reservation, DateOnly? date);
    Task ReserveDeskAsync(Desk desk, CreateReservationDto reservationDto);
}

public class ReservationService(IReservationsRepository reservationsRepository): IReservationService
{
    public async Task CancelReservationAsync(Reservation reservation, DateOnly? date)
    {
        if (!date.HasValue || date.Value == DateOnly.MinValue) {
            await reservationsRepository.DeleteAsync(reservation);
            return;
        }
        
        var today = DateOnly.FromDateTime(DateTime.Today);
        
        if (reservation.EndDate < today)
            throw new InvalidOperationException("You cannot cancel a reservation that has already ended.");
        
        if (date.HasValue && date.Value < today)
            throw new InvalidOperationException("You cannot cancel a date that is already in the past.");
        
        if(reservation.StartDate == reservation.EndDate && reservation.StartDate == date) 
        {
            await reservationsRepository.DeleteAsync(reservation);
            return;
        }
        
        if(reservation.StartDate == date) 
        {
            reservation.StartDate = reservation.StartDate.AddDays(1);
            await reservationsRepository.UpdateAsync(reservation);
            return;
        }
        
        if(reservation.EndDate == date) 
        {
            reservation.EndDate = reservation.EndDate.AddDays(-1);
            await reservationsRepository.UpdateAsync(reservation);
            return;
        }
        
        if(reservation.StartDate < date && reservation.EndDate > date) 
        {
            Reservation newReservation = new Reservation
            {
                StartDate = date.Value.AddDays(1),
                EndDate = reservation.EndDate,
                UserId = reservation.UserId,
                DeskId = reservation.DeskId
            };
            
            reservation.EndDate = date.Value.AddDays(-1);
            await reservationsRepository.AddAsync(newReservation);
            await reservationsRepository.UpdateAsync(reservation);
            return;
        }
        
        throw new InvalidOperationException("The provided date is not within the reservation period.");
    }
    
    public async Task ReserveDeskAsync(Desk desk, CreateReservationDto reservationDto)
    {
        if (desk.State == DeskState.Maintenance)
        {
            throw new InvalidOperationException("Desk is under maintenance and cannot be reserved.");
        }
        
        var reservations = await reservationsRepository.GetReservationsForDeskInRangeAsync(desk.Id, reservationDto.StartDate, reservationDto.EndDate);
        if (reservations.Any())
        {
            var overlappingDates = FindOverlappingDates(reservations, reservationDto.StartDate, reservationDto.EndDate);
            throw new InvalidOperationException($"The desk is already reserved on the following dates: {string.Join(", ", overlappingDates)}");
        }
        
        var reservation = new Reservation
        {
            StartDate = reservationDto.StartDate,
            EndDate = reservationDto.EndDate,
            UserId = reservationDto.UserId,
            DeskId = desk.Id
        };
        
        await reservationsRepository.AddAsync(reservation);
    }
    
    private List<DateOnly> FindOverlappingDates(IReadOnlyList<Reservation> reservations, DateOnly from, DateOnly to)
    {
        var overlappingDates = new List<DateOnly>();
        
        foreach (var reservation in reservations)
        {
            var start = reservation.StartDate > from ? reservation.StartDate : from;
            var end = reservation.EndDate < to ? reservation.EndDate : to;
            
            for (var date = start; date <= end; date = date.AddDays(1))
            {
                overlappingDates.Add(date);
            }
        }
        
        return overlappingDates;
    }
}