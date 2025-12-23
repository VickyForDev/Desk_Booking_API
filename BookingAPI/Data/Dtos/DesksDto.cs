using BookingAPI.Data.Models.Enums;

namespace BookingAPI.Data.Dtos;

public record DeskDto(
    int Id,
    DeskState State,
    IReadOnlyList<DeskReservationDto> Reservations
);