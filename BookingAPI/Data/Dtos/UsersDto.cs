namespace BookingAPI.Data.Dtos;

public record UserDto(
    int Id,
    string FirstName,
    string LastName
);

public record FullUserDto(
    int Id,
    string FirstName,
    string LastName,
    IReadOnlyList<UserReservationDto> Reservations
);