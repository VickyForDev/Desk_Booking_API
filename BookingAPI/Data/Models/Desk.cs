using BookingAPI.Data.Models.Enums;

namespace BookingAPI.Data.Models;

public class Desk
{
    public int Id { get; set; }
    public DeskState State { get; set; } = DeskState.Open;
    public ICollection<Reservation> Reservations { get; } = new List<Reservation>();
}