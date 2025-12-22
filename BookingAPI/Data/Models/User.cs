using System.ComponentModel.DataAnnotations;

namespace BookingAPI.Data.Models;

public class User
{
    public int Id { get; set; }
    [MaxLength(50)]
    public required string FirstName { get; set; }
    [MaxLength(50)]
    public required string LastName { get; set; }
    public ICollection<Reservation> Reservations { get; } = new List<Reservation>();
}