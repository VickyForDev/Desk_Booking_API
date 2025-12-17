using BookingAPI.Data.Models;

namespace BookingAPI.Data.Repositories;

public interface IReservationsRepository : IGenericRepository<Reservation>
{
    
}

public class ReservationsRepository(BookingDbContext dbContext): GenericRepository<Reservation>(dbContext), IReservationsRepository
{
    
}