namespace BookingAPI.Data;

public static class BookingDbSeeder
{
    public static async Task SeedAsync(BookingDbContext context)
    {
       await SeedUsersAsync(context);
       await SeedDesksAsync(context);
       await SeedReservationsAsync(context);

       await context.SaveChangesAsync();
    }
    
    private static Task SeedUsersAsync(BookingDbContext context)
    {
        return Task.CompletedTask;
    }
    
    private static Task SeedDesksAsync(BookingDbContext context)
    {
        return Task.CompletedTask;
    }
    
    private static Task SeedReservationsAsync(BookingDbContext context)
    {
        return Task.CompletedTask;
    }
}