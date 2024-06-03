using HotelManagementAPI.Models;
using HotelManagementAPI.Models.DTO;

namespace HotelManagementAPI.Data
{
    public class RoomStore
    {
        public static HotelManagementContext context = new HotelManagementContext();

        public static IEnumerable<RoomDTO> GetRooms(Hotel hotel)
        {
            return from room in context.Rooms
                   where room.HotelId == hotel.Id
                   select new RoomDTO
                   {
                       Id = room.Id,
                       RoomNumber = room.RoomNumber,
                       PricePerNight = room.PricePerNight,
                       Notes = room.Notes,
                       CurrencyFormat = context.Currencies.FirstOrDefault(x => x.Id == hotel.CurrencyId).FormattingString
                   };
        }
    }
}
