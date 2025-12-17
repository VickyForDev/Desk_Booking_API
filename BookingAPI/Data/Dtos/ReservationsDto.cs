namespace BookingAPI.Data.Dtos;

public record ReservationDto(
    int Id,
    DateTime StartDate,
    DateTime EndDate,
    int UserId,
    int DeskId
);