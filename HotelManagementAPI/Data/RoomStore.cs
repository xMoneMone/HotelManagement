using HotelManagementAPI.DataInterfaces;
using HotelManagementAPI.Models;
using HotelManagementAPI.Models.DTO;
using HotelManagementAPI.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementAPI.Data
{
    public class RoomStore(HotelManagementContext context, IUserStore userStore, IHotelStore hotelStore, ICurrencyStore currencyStore, IBedStore bedStore) : IRoomStore
    {
        private readonly HotelManagementContext context = context;
        private readonly IUserStore userStore = userStore;
        private readonly IHotelStore hotelStore = hotelStore;
        private readonly ICurrencyStore currencyStore = currencyStore;
        private readonly IBedStore bedStore = bedStore;

        public async Task<IActionResult> Add(RoomCreateDTO roomDTO, int hotelId)
        {
            var user = await userStore.GetCurrentUser();
            var hotel = await hotelStore.GetById(hotelId);
            var bedIds = await context.Beds.Select(x => x.Id).ToArrayAsync();

            var error = RoomValidators.CreateRoomValidator(user, roomDTO, hotel, bedIds);

            if (error != null)
            {
                return error;
            }
            var room = new Room
            {
                RoomNumber = roomDTO.RoomNumber,
                PricePerNight = roomDTO.PricePerNight,
                Notes = roomDTO.Notes,
                HotelId = hotelId
            };

            await context.Rooms.AddAsync(room);

            await context.SaveChangesAsync();

            foreach (int bedId in roomDTO.BedsIds)
            {
                await context.RoomsBeds.AddAsync(new RoomsBed
                {
                    BedId = bedId,
                    RoomId = room.Id
                });
            }

            await context.SaveChangesAsync();

            return new OkObjectResult(roomDTO);
        }

        public async Task<IActionResult> Edit(int roomId, int hotelId, RoomCreateDTO roomDTO)
        {
            var user = await userStore.GetCurrentUser();
            var room = await GetById(roomId);
            var hotel = await hotelStore.GetById(hotelId);
            var bedIds = await context.Beds.Select(x => x.Id).ToArrayAsync();

            var error = RoomValidators.EditRoomValidator(user, roomDTO, room, hotel, bedIds);

            if (error != null)
            {
                return error;
            }

            room.RoomNumber = roomDTO.RoomNumber;
            room.PricePerNight = roomDTO.PricePerNight;
            room.Notes = roomDTO.Notes;

            await context.SaveChangesAsync();
            return new OkObjectResult(roomDTO);
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

            return new ObjectResult("") { StatusCode = 204 };
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
            var hotel = await hotelStore.GetById(room?.HotelId);
            var employeesAtHotel = await hotelStore.GetHotelEmployeesIds(hotel?.Id);
            var beds = await bedStore.GetRoomBeds(id);

            var error = RoomValidators.GetRoomByIdValidator(user, room, hotel, employeesAtHotel);

            if (error != null)
            {
                return error;
            }
            
            return new OkObjectResult(new RoomDetailsDTO
            {
                Id = room.Id,
                RoomNumber = room.RoomNumber,
                PricePerNight = room.PricePerNight,
                Notes = room.Notes,
                Beds = beds
            });
        }

        public async Task<bool> RoomIsFree(int? roomId, DateTime start, DateTime end)
        {
            if (roomId == null)
            {
                return false;
            }
            var bookingsDuringTimePeriod = await (from booking in context.Bookings
                                                  where booking.RoomId == roomId &&
                                                        (booking.StartDate >= start && booking.StartDate <= end) ||
                                                        (booking.EndDate >= start && booking.EndDate <= end)
                                                  select booking).FirstOrDefaultAsync();
            return bookingsDuringTimePeriod == null;
        }

        public async Task<IEnumerable<Room>> All()
        {
            return await (from room in context.Rooms
                   select room).ToListAsync();
        }

        public async Task<IActionResult> GetRooms(int hotelId)
        {
            User? user = await userStore.GetCurrentUser();
            Hotel? hotel = await hotelStore.GetById(hotelId);
            int[] employeesAtHotel = await hotelStore.GetHotelEmployeesIds(hotel?.Id);

            IActionResult? error = RoomValidators.GetRoomsValidator(user, hotel, employeesAtHotel);

            if (error != null)
            {
                return error;
            }

            return new OkObjectResult(await (from roombed in context.RoomsBeds
                                             join room in context.Rooms on roombed.RoomId equals room.Id
                                             where room.HotelId == hotel.Id
                                             join bed in context.Beds on roombed.BedId equals bed.Id
                                             group room by room.Id into roomgroup
                                             select new RoomsDTO
                                             {
                                                 Id = roomgroup.Key,
                                                 RoomNumber = roomgroup.First().RoomNumber,
                                                 PricePerNight = roomgroup.First().PricePerNight,
                                                 Notes = roomgroup.First().Notes,
                                                 Capacity = 0
                                             }).ToListAsync());
        }
    }
}
