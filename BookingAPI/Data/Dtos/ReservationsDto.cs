using FluentValidation;

namespace BookingAPI.Data.Dtos;

public record ReservationDto(
    int Id,
    DateOnly StartDate,
    DateOnly EndDate
);

public record UserReservationDto(
    int Id,
    DateOnly StartDate,
    DateOnly EndDate,
    int DeskId
);

public record DeskReservationDto(
    int Id,
    DateOnly StartDate,
    DateOnly EndDate,
    UserDto User
);

public record CancelReservationDto(
    DateOnly Date
);

public record CreateReservationDto(
    DateOnly StartDate,
    DateOnly EndDate,
    int UserId
);

public class CreateReservationDtoValidator : AbstractValidator<CreateReservationDto> 
{
    public CreateReservationDtoValidator() 
    {
        RuleFor(x => x.StartDate).NotNull().WithMessage("Start date is required.")
            .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow))
            .WithMessage("Start date must be today or in the future.");

        RuleFor(x => x.EndDate).NotNull().WithMessage("End date is required.")
            .GreaterThan(x => x.StartDate)
            .WithMessage("End date must be after start date.");

        RuleFor(x => x.UserId).NotNull().WithMessage("User id is required.");
    }
}
