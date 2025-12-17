using BookingAPI.Data.Models;

namespace BookingAPI.Data.Repositories;

public interface IDesksRepository : IGenericRepository<Desk>
{
    
}

public class DesksRepository(BookingDbContext dbContext) : GenericRepository<Desk>(dbContext), IDesksRepository
{
    
}