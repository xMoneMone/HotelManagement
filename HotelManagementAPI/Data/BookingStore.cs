using HotelManagementAPI.DataInterfaces;
using HotelManagementAPI.Models;
using HotelManagementAPI.Models.DTO;
using HotelManagementAPI.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementAPI.Data
{
    public class BookingStore(HotelManagementContext context, IUserStore userStore, IRoomStore roomStore, IHotelStore hotelStore) : IBookingStore
    {
        private readonly HotelManagementContext context = context;
        private readonly IUserStore userStore = userStore;
        private readonly IRoomStore roomStore = roomStore;
        private readonly IHotelStore hotelStore = hotelStore;

        public async Task<IActionResult> Add(BookingCreateDTO bookingDTO, int roomId)
        {
            var user = await userStore.GetCurrentUser();
            var room = await roomStore.GetById(roomId);
            var hotel = await hotelStore.GetById(room?.HotelId);
            var employeesAtHotel = await hotelStore.GetHotelEmployeesIds(hotel?.Id);

            var error = BookingValidators.CreateBookingValidator(user, bookingDTO, room, hotel, employeesAtHotel);

            if (error != null)
            {
                return error;
            }

            await context.AddAsync(new Booking
            {
                FirstName = bookingDTO.FirstName,
                LastName = bookingDTO.LastName,
                StartDate = bookingDTO.StartDate,
                EndDate = bookingDTO.EndDate,
                DownPaymentPaid = bookingDTO.DownPaymentPaid,
                FullPaymentPaid = bookingDTO.FullPaymentPaid,
                DownPaymentPrice = bookingDTO.DownPaymentPrice,
                FullPaymentPrice = bookingDTO.FullPaymentPrice,
                Notes = bookingDTO.Notes,
                RoomId = roomId
            });
            await context.SaveChangesAsync();

            return new OkObjectResult(bookingDTO);
        }

        public async Task<IActionResult> Edit(int id, BookingCreateDTO bookingDTO)
        {
            var booking = await GetById(id);
            var room = await roomStore.GetById(booking?.RoomId);
            var hotel = await hotelStore.GetById(room?.HotelId);
            var employeesAtHotel = await hotelStore.GetHotelEmployeesIds(hotel?.Id);

            var user = await userStore.GetCurrentUser();

            var error = BookingValidators.EditBookingValidator(user, bookingDTO, booking, room, hotel, employeesAtHotel);

            if (error != null)
            {
                return error;
            }

            booking.FirstName = bookingDTO.FirstName;
            booking.LastName = bookingDTO.LastName;
            booking.StartDate = bookingDTO.StartDate;
            booking.EndDate = bookingDTO.EndDate;
            booking.DownPaymentPaid = bookingDTO.DownPaymentPaid;
            booking.FullPaymentPaid = bookingDTO.FullPaymentPaid;
            booking.DownPaymentPrice = bookingDTO.DownPaymentPrice;
            booking.FullPaymentPrice = bookingDTO.FullPaymentPrice;
            booking.Notes = bookingDTO.Notes;
            await context.SaveChangesAsync();

            return new OkObjectResult(bookingDTO);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var user = await userStore.GetCurrentUser();
            var booking = await GetById(id);
            var room = await roomStore.GetById(booking?.RoomId);
            var hotel = await hotelStore.GetById(room?.HotelId);

            var error = BookingValidators.DeleteBookingValidator(user, booking, hotel.OwnerId);

            if (error != null)
            {
                return error;
            }

            context.Bookings.Remove(booking);
            await context.SaveChangesAsync();

            return new ObjectResult("") { StatusCode = 204};
        }

        public async Task<Booking?> GetById(int? id)
        {
            if (id == null)
            {
                return null;
            }

            return await (from booking in context.Bookings
                          where id == booking.Id
                          select booking)
                   .FirstOrDefaultAsync();
        }

        public async Task<IActionResult> GetDTOById(int id)
        {
            var user = await userStore.GetCurrentUser();
            var booking = await GetById(id);
            var room = await roomStore.GetById(booking?.RoomId);
            var hotel = await hotelStore.GetById(room?.HotelId);
            var employeesAtHotel = await hotelStore.GetHotelEmployeesIds(hotel?.Id);

            var error = BookingValidators.GetBookingByIdValidator(user, booking, room, hotel, employeesAtHotel);

            if (error != null)
            {
                return error;
            }

            return new OkObjectResult(await (from bkng in context.Bookings
                                             where id == bkng.Id
                                             select new BookingDTO
                                             {
                                                 Id = bkng.Id,
                                                 FirstName = bkng.FirstName,
                                                 LastName = bkng.LastName,
                                                 StartDate = bkng.StartDate,
                                                 EndDate = bkng.EndDate,
                                                 DownPaymentPaid = bkng.DownPaymentPaid,
                                                 FullPaymentPaid = bkng.FullPaymentPaid,
                                                 DownPaymentPrice = bkng.DownPaymentPrice,
                                                 FullPaymentPrice = bkng.FullPaymentPrice,
                                                 Notes = bkng.Notes,
                                                 RoomId = bkng.RoomId
                                             })
                   .FirstOrDefaultAsync());
        }

        public async Task<IEnumerable<Booking>> All()
        {
            return await (from booking in context.Bookings
                          select booking).ToListAsync();
        }

        public async Task<IActionResult> GetBookings(int roomId)
        {
            var user = await userStore.GetCurrentUser();
            var room = await roomStore.GetById(roomId);
            var hotel = await hotelStore.GetById(room?.HotelId);
            var employeesAtHotel = await hotelStore.GetHotelEmployeesIds(hotel?.Id);

            var error = BookingValidators.GetBookingsValidator(user, room, hotel, employeesAtHotel);

            if (error != null)
            {
                return error;
            }

            return new OkObjectResult(await (from booking in context.Bookings
                                             where booking.RoomId == room.Id
                                             select new BookingDTO
                                             {
                                                 Id = booking.Id,
                                                 FirstName = booking.FirstName,
                                                 LastName = booking.LastName,
                                                 StartDate = booking.StartDate,
                                                 EndDate = booking.EndDate,
                                                 DownPaymentPaid = booking.DownPaymentPaid,
                                                 FullPaymentPaid = booking.FullPaymentPaid,
                                                 DownPaymentPrice = booking.DownPaymentPrice,
                                                 FullPaymentPrice = booking.FullPaymentPrice,
                                                 Notes = booking.Notes,
                                                 RoomId = room.Id
                                             }).ToListAsync());
        }
    }
}
