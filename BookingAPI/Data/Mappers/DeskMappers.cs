using BookingAPI.Data.Dtos;
using BookingAPI.Data.Models;

namespace BookingAPI.Data.Mappers;

public static class DeskMappers
{
    public static DeskDto ToDeskDto(this Desk desk) =>
        new DeskDto(
            desk.Id,
            desk.State
        );

    public static Desk ToDesk(this DeskDto deskDto) =>
        new Desk
        {
            State = deskDto.State
        };
}