using HotelManagementAPI.DataInterfaces;
using HotelManagementAPI.Models;
using HotelManagementAPI.Models.DTO;
using HotelManagementAPI.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementAPI.Data
{
    public class RoomStore(HotelManagementContext context, IUserStore userStore, IHotelStore hotelStore, ICurrencyStore currencyStore) : IRoomStore
    {
        private readonly HotelManagementContext context = context;
        private readonly IUserStore userStore = userStore;
        private readonly IHotelStore hotelStore = hotelStore;
        private readonly ICurrencyStore currencyStore = currencyStore;

        public async Task<IActionResult> Add(RoomCreateDTO roomDTO, int hotelId)
        {
            var user = await userStore.GetCurrentUser();
            var hotel = await hotelStore.GetById(roomDTO.HotelId);

            var error = RoomValidators.CreateRoomValidator(user, roomDTO, hotel);

            if (error != null)
            {
                return error;
            }

            await context.Rooms.AddAsync(new Room
            {
                RoomNumber = roomDTO.RoomNumber,
                PricePerNight = roomDTO.PricePerNight,
                Notes = roomDTO.Notes,
                HotelId = hotelId
            });
            await context.SaveChangesAsync();
            return new OkObjectResult("Room created successfully.");
        }

        public async Task<IActionResult> Edit(int id, RoomCreateDTO roomDTO)
        {
            var user = await userStore.GetCurrentUser();
            var room = await GetById(id);
            var hotel = await hotelStore.GetById(roomDTO.HotelId);

            var error = RoomValidators.EditRoomValidator(user, roomDTO, room, hotel);

            if (error != null)
            {
                return error;
            }

            room.RoomNumber = roomDTO.RoomNumber;
            room.PricePerNight = roomDTO.PricePerNight;
            room.Notes = roomDTO.Notes;

            await context.SaveChangesAsync();
            return new OkObjectResult("Room created successfully.");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var user = await userStore.GetCurrentUser();
            var room = await GetById(id);
            var hotel = await hotelStore.GetById(room?.HotelId);

            var error = RoomValidators.DeleteRoomValidator(user, room, hotel);

            if (error != null)
            {
                return error;
            }

            context.Rooms.Remove(room);
            await context.SaveChangesAsync();

            return new OkObjectResult("Room has been deleted.");
        }

        public async Task<Room?> GetById(int? id)
        {
            if (id == null)
            {
                return null;
            }
            return await (from room in context.Rooms
                    where id == room.Id
                    select room)
                   .FirstOrDefaultAsync();
        }

        public async Task<IActionResult> GetDTOById(int id)
        {
            var user = await userStore.GetCurrentUser();
            var room = await GetById(id);
            var hotel = await hotelStore.GetById(room.HotelId);
            var employeesAtHotel = await hotelStore.GetHotelEmployeesIds(hotel?.Id);
            var currency = await currencyStore.GetById(hotel.CurrencyId);

            var error = RoomValidators.GetRoomByIdValidator(user, room, hotel, employeesAtHotel);

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
                CurrencyFormat = currency.FormattingString
            });
        }

        public async Task<IEnumerable<Room>> All()
        {
            return await (from room in context.Rooms
                   select room).ToListAsync();
        }

        public async Task<IActionResult> GetRooms(int hotelId)
        {
            var user = await userStore.GetCurrentUser();
            var hotel = await hotelStore.GetById(hotelId);
            var employeesAtHotel = await hotelStore.GetHotelEmployeesIds(hotel?.Id);

            var error = RoomValidators.GetRoomsValidator(user, hotel, employeesAtHotel);

            if (error != null)
            {
                return error;
            }

            return new OkObjectResult(await (from room in context.Rooms
                                      where room.HotelId == hotel.Id
                                      select new RoomDTO
                                      {
                                          Id = room.Id,
                                          RoomNumber = room.RoomNumber,
                                          PricePerNight = room.PricePerNight,
                                          Notes = room.Notes,
                                          CurrencyFormat = currencyStore.GetById(hotel.CurrencyId).FormattingString
                                      }).ToListAsync());
        }
    }
}
