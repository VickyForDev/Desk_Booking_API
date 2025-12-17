using BookingAPI.Data.Dtos;
using BookingAPI.Data.Models;

namespace BookingAPI.Data.Mappers;

public static class ReservationMappers
{
    public static ReservationDto ToReservationDto(this Reservation reservation) =>
        new ReservationDto(
            reservation.Id,
            reservation.StartDate,
            reservation.EndDate,
            reservation.UserId,
            reservation.DeskId
        );
    
    public static Reservation ToReservation(this ReservationDto reservationDto) =>
        new Reservation{
            StartDate = reservationDto.StartDate,
            EndDate = reservationDto.EndDate,
            UserId = reservationDto.UserId,
            DeskId = reservationDto.DeskId
        };
}