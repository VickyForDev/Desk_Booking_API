using BookingAPI.Data.Models;
using BookingAPI.Data.Models.Enums;

namespace BookingAPI.Data;

public static class BookingDbSeeder
{
    public static async Task SeedAsync(BookingDbContext context)
    {
       if (context.Users.Any()) return;
        
       var users = await SeedUsersAsync(context);
       var desks = await SeedDesksAsync(context);
       
       await context.SaveChangesAsync();
       
       await SeedReservationsAsync(context, users, desks);

       await context.SaveChangesAsync();
    }
    
    private static Task<List<User>> SeedUsersAsync(BookingDbContext context)
    {
        var users = new List<User>
        {
            new () { FirstName = "John", LastName = "Doe" },
            new () { FirstName = "Jane", LastName = "Doe" },
            new () { FirstName = "Alice", LastName = "Doe" }
        };
        
        context.Users.AddRange(users);
        return Task.FromResult(users);
    }
    
    private static Task<List<Desk>> SeedDesksAsync(BookingDbContext context)
    {
        var desks = new List<Desk>
        {
            new (),
            new () {State = DeskState.Maintenance},
            new () {State = DeskState.Maintenance},
            new (),
            new (),
            new (),
            new (),
            new () {State = DeskState.Maintenance},
            new (),
            new ()
        };
        
        context.AddRange(desks);
        return Task.FromResult(desks);
    }
    
    private static Task SeedReservationsAsync(BookingDbContext context, List<User> users, List<Desk> desks)
    {
        context.AddRange(
            new Reservation { StartDate = DateTime.Today.AddDays(1), EndDate = DateTime.Today.AddDays(3), User = users[0], Desk = desks[0] },
            new Reservation { StartDate = DateTime.Today.AddDays(4), EndDate = DateTime.Today.AddDays(5), User = users[0], Desk = desks[3] },
            new Reservation { StartDate = DateTime.Today.AddDays(4), EndDate = DateTime.Today.AddDays(4), User = users[1], Desk = desks[0] },
            new Reservation { StartDate = DateTime.Today, EndDate = DateTime.Today.AddDays(2), User = users[2], Desk = desks[4] },
            new Reservation { StartDate = DateTime.Today, EndDate = DateTime.Today, User = users[1], Desk = desks[8] }
        );
        
        return Task.CompletedTask;
    }
}