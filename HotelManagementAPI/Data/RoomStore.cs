using HotelManagementAPI.DataInterfaces;
using HotelManagementAPI.Models;
using HotelManagementAPI.Models.DTO;
using HotelManagementAPI.Util;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementAPI.Data
{
    public class RoomStore(HotelManagementContext context, IUserStore userStore, IHotelStore hotelStore) : IRoomStore
    {
        private readonly HotelManagementContext context = context;
        private readonly IUserStore userStore = userStore;
        private readonly IHotelStore hotelStore = hotelStore;

        public IActionResult Add(RoomCreateDTO roomDTO, int hotelId)
        {
            var user = userStore.GetCurrentUser();
            var hotel = hotelStore.GetById(roomDTO.HotelId);

            var error = RoomValidators.CreateRoomValidator(user, roomDTO, hotel);

            if (error != null)
            {
                return error;
            }

            context.Rooms.Add(new Room
            {
                RoomNumber = roomDTO.RoomNumber,
                PricePerNight = roomDTO.PricePerNight,
                Notes = roomDTO.Notes,
                HotelId = hotelId
            });
            context.SaveChanges();
            return new OkObjectResult("Room created successfully.");
        }

        public IActionResult Edit(int id, RoomCreateDTO roomDTO)
        {
            var user = userStore.GetCurrentUser();
            var room = GetById(id);
            var hotel = hotelStore.GetById(roomDTO.HotelId);

            var error = RoomValidators.EditRoomValidator(user, roomDTO, room, hotel);

            if (error != null)
            {
                return error;
            }

            room.RoomNumber = roomDTO.RoomNumber;
            room.PricePerNight = roomDTO.PricePerNight;
            room.Notes = roomDTO.Notes;

            context.SaveChanges();
            return new OkObjectResult("Room created successfully.");
        }

        public IActionResult Delete(int id)
        {
            var user = userStore.GetCurrentUser();
            var room = GetById(id);
            var hotel = hotelStore.GetById(room?.HotelId);

            var error = RoomValidators.DeleteRoomValidator(user, room, hotel);

            if (error != null)
            {
                return error;
            }

            context.Rooms.Remove(room);
            context.SaveChanges();

            return new OkObjectResult("Room has been deleted.");
        }

        public Room? GetById(int id)
        {
            return (from room in context.Rooms
                    where id == room.Id
                    select room)
                   .FirstOrDefault();
        }

        public IActionResult GetDTOById(int id)
        {
            var user = userStore.GetCurrentUser();
            var room = GetById(id);
            var hotel = hotelStore.GetById(room.HotelId);

            var error = RoomValidators.GetRoomByIdValidator(user, room, hotel);

            if (error != null)
            {
                return error;
            }

            return new OkObjectResult(new RoomDTO
            {
                Id = room.Id,
                RoomNumber = room.RoomNumber,
                PricePerNight = room.PricePerNight,
                Notes = room.Notes,
                CurrencyFormat = CurrencyStore.GetById(hotel.CurrencyId).FormattingString
            });
        }

        public IEnumerable<Room> All()
        {
            return from room in context.Rooms
                   select room;
        }

        public IActionResult GetRooms(int hotelId)
        {
            var user = userStore.GetCurrentUser();
            var hotel = hotelStore.GetById(hotelId);

            var error = RoomValidators.GetRoomsValidator(user, hotel);

            if (error != null)
            {
                return error;
            }

            return new OkObjectResult(from room in context.Rooms
                                      where room.HotelId == hotel.Id
                                      select new RoomDTO
                                      {
                                          Id = room.Id,
                                          RoomNumber = room.RoomNumber,
                                          PricePerNight = room.PricePerNight,
                                          Notes = room.Notes,
                                          fix later
                                          CurrencyFormat = context.Currencies.FirstOrDefault(x => x.Id == hotel.CurrencyId).FormattingString
                                      });
        }
    }
}
