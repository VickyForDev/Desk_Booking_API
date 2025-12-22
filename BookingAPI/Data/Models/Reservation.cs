namespace BookingAPI.Data.Models;

public class Reservation
{
    public int Id { get; set; }
    public required DateOnly StartDate { get; set; }
    public required DateOnly EndDate { get; set; }
    
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public int DeskId { get; set; }
    public Desk Desk { get; set; } = null!;
}