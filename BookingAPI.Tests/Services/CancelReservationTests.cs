using BookingAPI.Data.Models;
using BookingAPI.Data.Repositories;
using BookingAPI.Services;
using FluentAssertions;
using Moq;

namespace BookingAPI.Tests.Services;

public class CancelReservationTests
{
    private readonly Mock<IReservationsRepository> _repoMock;
    private readonly IReservationService _service;
    private readonly DateOnly _today;

    public CancelReservationTests()
    {
        _repoMock = new Mock<IReservationsRepository>();
        _service = new ReservationService(_repoMock.Object);
        _today = DateOnly.FromDateTime(DateTime.Today);
    }

    [Fact]
    public async Task CancelReservationAsync_ShouldThrow_WhenReservationAlreadyEnded()
    {
        var reservation = new Reservation
        {
            StartDate = _today.AddDays(-5),
            EndDate = _today.AddDays(-1)
        };

        Func<Task> act = () => _service.CancelReservationAsync(reservation, _today);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("You cannot cancel a reservation that has already ended.");
    }

    [Fact]
    public async Task CancelReservationAsync_ShouldDelete_WhenDateIsNull()
    {
        var reservation = new Reservation
        {
            StartDate = _today,
            EndDate = _today.AddDays(3)
        };

        await _service.CancelReservationAsync(reservation, null);

        _repoMock.Verify(r => r.DeleteAsync(reservation), Times.Once);
    }

    [Fact]
    public async Task CancelReservationAsync_ShouldThrow_WhenDateIsInThePast()
    {
        var reservation = new Reservation
        {
            StartDate = _today,
            EndDate = _today.AddDays(3)
        };

        var pastDate = _today.AddDays(-1);

        Func<Task> act = () => _service.CancelReservationAsync(reservation, pastDate);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("You cannot cancel a date that is already in the past.");
    }

    [Fact]
    public async Task CancelReservationAsync_ShouldDelete_WhenSingleDayReservationCancelled()
    {
        var reservation = new Reservation
        {
            StartDate = _today,
            EndDate = _today
        };

        await _service.CancelReservationAsync(reservation, _today);

        _repoMock.Verify(r => r.DeleteAsync(reservation), Times.Once);
    }

    [Fact]
    public async Task CancelReservationAsync_ShouldShortenStart_WhenCancellingStartDate()
    {
        var reservation = new Reservation
        {
            StartDate = _today,
            EndDate = _today.AddDays(5)
        };

        await _service.CancelReservationAsync(reservation, _today);

        reservation.StartDate.Should().Be(_today.AddDays(1));
        _repoMock.Verify(r => r.UpdateAsync(reservation), Times.Once);
    }

    [Fact]
    public async Task CancelReservationAsync_ShouldShortenEnd_WhenCancellingEndDate()
    {
        var reservation = new Reservation
        {
            StartDate = _today,
            EndDate = _today.AddDays(5)
        };

        var endDate = _today.AddDays(5);

        await _service.CancelReservationAsync(reservation, endDate);

        reservation.EndDate.Should().Be(endDate.AddDays(-1));
        _repoMock.Verify(r => r.UpdateAsync(reservation), Times.Once);
    }

    [Fact]
    public async Task CancelReservationAsync_ShouldSplitReservation_WhenCancellingMiddleDate()
    {
        var reservation = new Reservation
        {
            StartDate = _today,
            EndDate = _today.AddDays(5),
            UserId = 1,
            DeskId = 2
        };

        var cancelDate = _today.AddDays(2);

        await _service.CancelReservationAsync(reservation, cancelDate);
        
        reservation.EndDate.Should().Be(cancelDate.AddDays(-1));
        
        _repoMock.Verify(r => r.AddAsync(It.Is<Reservation>(re =>
            re.StartDate == cancelDate.AddDays(1) &&
            re.EndDate == _today.AddDays(5) &&
            re.UserId == 1 &&
            re.DeskId == 2
        )), Times.Once);

        _repoMock.Verify(r => r.UpdateAsync(reservation), Times.Once);
    }

    [Fact]
    public async Task CancelReservationAsync_ShouldThrow_WhenDateNotInRange()
    {
        var reservation = new Reservation
        {
            StartDate = _today,
            EndDate = _today.AddDays(5)
        };

        var invalidDate = _today.AddDays(10);

        Func<Task> act = () => _service.CancelReservationAsync(reservation, invalidDate);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("The provided date is not within the reservation period.");
    }
}
