using BookingAPI.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace BookingAPI.Data.Repositories;

public interface IDesksRepository : IGenericRepository<Desk>
{
    Task<IReadOnlyList<Desk>> GetAllByDateRangeAsync(DateOnly? from, DateOnly? to);
}

public class DesksRepository(BookingDbContext dbContext) : GenericRepository<Desk>(dbContext), IDesksRepository
{
    public async Task<IReadOnlyList<Desk>> GetAllByDateRangeAsync(DateOnly? from, DateOnly? to)
    {
        var start = from ?? DateOnly.FromDateTime(DateTime.Today);
        var end = to ?? start;

        return await dbContext.Desks
            .Include(d => d.Reservations
                .Where(r => r.StartDate <= end && r.EndDate >= start)
            )
            .ThenInclude(r => r.User)
            .ToListAsync();
    }
}