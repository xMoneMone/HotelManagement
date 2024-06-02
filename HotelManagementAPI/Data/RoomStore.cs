using HotelManagementAPI.Models;
using HotelManagementAPI.Models.DTO;

namespace HotelManagementAPI.Data
{
    public class RoomStore
    {
        public static HotelManagementContext context = new HotelManagementContext();

        public static IEnumerable<RoomDTO> GetRooms(int hotelId)
        {
            return from room in context.Rooms
                   where room.HotelId == hotelId
                   select new RoomDTO
                   {
                       Id = room.Id,
                       RoomNumber = room.RoomNumber,
                       PricePerNight = room.PricePerNight,
                       Notes = room.Notes
                   };
        }
    }
}
