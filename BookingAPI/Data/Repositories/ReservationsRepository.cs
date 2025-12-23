using BookingAPI.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace BookingAPI.Data.Repositories;

public interface IReservationsRepository : IGenericRepository<Reservation>
{
    Task<IReadOnlyList<Reservation>> GetReservationsForDeskInRangeAsync(int deskId, DateOnly from, DateOnly to);
    Task<IReadOnlyList<Reservation>> GetActiveReservationsForDeskAsync(int deskId);
}

public class ReservationsRepository(BookingDbContext dbContext): GenericRepository<Reservation>(dbContext), IReservationsRepository
{
    public override async Task<IReadOnlyList<Reservation>> GetAllAsync()
    {
        return await dbContext.Reservations
            .Include(r => r.User)
            .Include(r => r.Desk)
            .ToListAsync();
    }

    public override async Task<Reservation?> GetByIdAsync(int id)
    {
        return await dbContext.Reservations
            .Include(r => r.User)
            .Include(r => r.Desk)
            .FirstOrDefaultAsync(r => r.Id == id);
    }
    
    public async Task<IReadOnlyList<Reservation>> GetReservationsForDeskInRangeAsync(int deskId, DateOnly from, DateOnly to)
    {
        return await dbContext.Reservations
            .Where(r => r.DeskId == deskId 
                        && r.StartDate <= to 
                        && r.EndDate >= from)
            .ToListAsync();
    }
    
    public async Task<IReadOnlyList<Reservation>> GetActiveReservationsForDeskAsync(int deskId)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        return await dbContext.Reservations
            .Where(r => r.DeskId == deskId && r.EndDate >= today)
            .ToListAsync();
    }
}