using BookingAPI.Data.Dtos;
using BookingAPI.Data.Models;
using BookingAPI.Data.Models.Enums;
using BookingAPI.Data.Repositories;
using BookingAPI.Services;
using FluentAssertions;
using Moq;

namespace BookingAPI.Tests.Services;

public class ReserveDeskTests
{
    private readonly Mock<IReservationsRepository> _repoMock;
    private readonly IReservationService _service;

    public ReserveDeskTests()
    {
        _repoMock = new Mock<IReservationsRepository>();
        _service = new ReservationService(_repoMock.Object);
    }

    [Fact]
    public async Task ReserveDeskAsync_ShouldThrow_WhenDeskIsUnderMaintenance()
    {
        var desk = new Desk { Id = 1, State = DeskState.Maintenance };
        var dto = new CreateReservationDto
        (
            new DateOnly(2025, 1, 10),
            new DateOnly(2025, 1, 12),
            5
        );

        Func<Task> act = () => _service.ReserveDeskAsync(desk, dto);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Desk is under maintenance and cannot be reserved.");
    }
    
    [Fact]
    public async Task ReserveDeskAsync_ShouldThrowWithCorrectOverlappingDates_WhenOverlapExists()
    {
        var desk = new Desk { Id = 1, State = DeskState.Open };

        var dto = new CreateReservationDto(
            new DateOnly(2025, 1, 10),
            new DateOnly(2025, 1, 12),
            5
        );

        var existingReservations = new List<Reservation>
        {
            new ()
            {
                StartDate = new DateOnly(2025, 1, 11),
                EndDate = new DateOnly(2025, 1, 13)
            }
        };

        _repoMock
            .Setup(r => r.GetReservationsForDeskInRangeAsync(1, dto.StartDate, dto.EndDate))
            .ReturnsAsync(existingReservations);
        
        Func<Task> act = () => _service.ReserveDeskAsync(desk, dto);
        
        var ex = await act.Should().ThrowAsync<InvalidOperationException>();

        ex.Which.Message.Should().Contain("2025-01-11");
        ex.Which.Message.Should().Contain("2025-01-12");
        ex.Which.Message.Should().NotContain("2025-01-13");
    }
    
    [Fact]
    public async Task ReserveDeskAsync_ShouldAddReservation_WhenNoConflictsExist()
    {
        var desk = new Desk { Id = 1, State = DeskState.Open };

        var dto = new CreateReservationDto
        (
            new DateOnly(2025, 1, 10),
            new DateOnly(2025, 1, 12),
            5
        );

        _repoMock
            .Setup(r => r.GetReservationsForDeskInRangeAsync(1, dto.StartDate, dto.EndDate))
            .ReturnsAsync(new List<Reservation>());

        await _service.ReserveDeskAsync(desk, dto);

        _repoMock.Verify(r => r.AddAsync(It.Is<Reservation>(res =>
            res.StartDate == dto.StartDate &&
            res.EndDate == dto.EndDate &&
            res.UserId == dto.UserId &&
            res.DeskId == desk.Id
        )), Times.Once);
    }
}