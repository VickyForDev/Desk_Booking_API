using BookingAPI.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace BookingAPI.Data.Repositories;

public interface IUsersRepository : IGenericRepository<User>
{
    
}

public class UsersRepository(BookingDbContext dbContext) : GenericRepository<User>(dbContext), IUsersRepository
{
    public override async Task<User?> GetByIdAsync(int id)
    {
        return await dbContext.Users
            .Include(r => r.Reservations)
            .ThenInclude(r => r.Desk)
            .FirstOrDefaultAsync(r => r.Id == id);
    }
}