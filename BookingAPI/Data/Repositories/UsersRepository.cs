using BookingAPI.Data.Models;

namespace BookingAPI.Data.Repositories;

public interface IUsersRepository : IGenericRepository<User>
{
    
}

public class UsersRepository(BookingDbContext dbContext) : GenericRepository<User>(dbContext), IUsersRepository
{
    
}