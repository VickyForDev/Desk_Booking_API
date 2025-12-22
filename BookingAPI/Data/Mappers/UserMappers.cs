using BookingAPI.Data.Dtos;
using BookingAPI.Data.Models;

namespace BookingAPI.Data.Mappers;

public static class UserMappers
{
    public static UserDto ToUserDto(this User user) =>
        new UserDto(
            user.Id,
            user.FirstName,
            user.LastName
        );
    
    public static FullUserDto ToFullUserDto(this User user) =>
        new FullUserDto(
            user.Id,
            user.FirstName,
            user.LastName,
            user.Reservations.Select(r => r.ToUserReservationDto()).ToList()   
        );
}