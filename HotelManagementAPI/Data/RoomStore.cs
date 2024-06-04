using HotelManagementAPI.Models;
using HotelManagementAPI.Models.DTO;

namespace HotelManagementAPI.Data
{
    public class RoomStore : DataStore
    {
        public static void Add(RoomCreateDTO roomDTO, int hotelId, User user)
        {
            context.Rooms.Add(new Room
            {
                RoomNumber = roomDTO.RoomNumber,
                PricePerNight = roomDTO.PricePerNight,
                Notes = roomDTO.Notes,
                HotelId = hotelId
            });
            context.SaveChanges();
        }

        public static void Edit(int id, RoomCreateDTO roomDTO)
        {
            var room = GetById(id);

            room.RoomNumber = roomDTO.RoomNumber;
            room.PricePerNight = roomDTO.PricePerNight;
            room.Notes = roomDTO.Notes;

            context.SaveChanges();
        }

        public static void Delete(int id)
        {
            var room = GetById(id);
            context.Rooms.Remove(room);
            context.SaveChanges();
        }

        public static Room? GetById(int id)
        {
            return (from room in context.Rooms
                    where id == room.Id
                    select room)
                   .FirstOrDefault();
        }

        public static RoomDTO? GetDTOById(int id)
        {
            var room = GetById(id);
            var hotel = HotelStore.GetById(room.HotelId);

            return new RoomDTO
            {
                Id = room.Id,
                RoomNumber = room.RoomNumber,
                PricePerNight = room.PricePerNight,
                Notes = room.Notes,
                CurrencyFormat = CurrencyStore.GetById(hotel.CurrencyId).FormattingString
            };
        }

        public static IEnumerable<Room> All()
        {
            return from room in context.Rooms
                   select room;
        }

        public static IEnumerable<RoomDTO> GetRooms(int hotelId)
        {
            var hotel = HotelStore.GetById(hotelId);
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
