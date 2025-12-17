using BookingAPI.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace BookingAPI.Data.Repositories;

public interface IReservationsRepository : IGenericRepository<Reservation>
{
    
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
}