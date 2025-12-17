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

    public static User ToUser(this UserDto userDto) =>
        new User
        {
            FirstName = userDto.FirstName,
            LastName = userDto.LastName
        };
}