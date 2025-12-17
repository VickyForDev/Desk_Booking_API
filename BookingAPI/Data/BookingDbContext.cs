using BookingAPI.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace BookingAPI.Data;

public class BookingDbContext : DbContext
{
    public BookingDbContext(DbContextOptions<BookingDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Desk> Desks { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
}