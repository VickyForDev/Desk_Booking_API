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
}