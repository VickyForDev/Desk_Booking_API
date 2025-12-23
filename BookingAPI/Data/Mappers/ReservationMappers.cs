using BookingAPI.Data.Dtos;
using BookingAPI.Data.Models;

namespace BookingAPI.Data.Mappers;

public static class ReservationMappers
{
    public static ReservationDto ToReservationDto(this Reservation reservation) =>
        new ReservationDto(
            reservation.Id,
            reservation.StartDate,
            reservation.EndDate
        );
    
    public static UserReservationDto ToUserReservationDto(this Reservation reservation) =>
        new UserReservationDto(
            reservation.Id,
            reservation.StartDate,
            reservation.EndDate,
            reservation.DeskId
        );
    
    public static DeskReservationDto ToDeskReservationDto(this Reservation reservation) =>
        new DeskReservationDto(
            reservation.Id,
            reservation.StartDate,
            reservation.EndDate,
            reservation.User.ToUserDto()
        );
}