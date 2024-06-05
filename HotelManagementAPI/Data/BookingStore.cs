using HotelManagementAPI.DataInterfaces;
using HotelManagementAPI.Models;
using HotelManagementAPI.Models.DTO;
using HotelManagementAPI.Util;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementAPI.Data
{
    public class BookingStore(HotelManagementContext context, IUserStore userStore, IRoomStore roomStore, IHotelStore hotelStore) : IBookingStore
    {
        private readonly HotelManagementContext context = context;
        private readonly IUserStore userStore = userStore;
        private readonly IRoomStore roomStore = roomStore;
        private readonly IHotelStore hotelStore = hotelStore;

        public IActionResult Add(BookingCreateDTO bookingDTO, int roomId)
        {
            var user = userStore.GetCurrentUser();
            var room = roomStore.GetById(roomId);
            var hotel = hotelStore.GetById(room?.HotelId);

            var error = BookingValidators.CreateBookingValidator(user, bookingDTO, room, hotel);

            if (error != null)
            {
                return error;
            }

            context.Add(new Booking
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
            context.SaveChanges();

            return new OkObjectResult("Booking created successfully.");
        }

        public IActionResult Edit(int id, BookingCreateDTO bookingDTO)
        {
            var booking = GetById(id);
            var room = roomStore.GetById(booking?.RoomId);
            var hotel = hotelStore.GetById(room?.HotelId);

            var user = userStore.GetCurrentUser();

            var error = BookingValidators.EditBookingValidator(user, bookingDTO, booking, room, hotel);

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
            context.SaveChanges();

            return new OkObjectResult("Booking edited successfully.");
        }

        public IActionResult Delete(int id)
        {
            var user = userStore.GetCurrentUser();
            var booking = GetById(id);
            var room = roomStore.GetById(booking?.RoomId);
            var hotel = hotelStore.GetById(room?.HotelId);

            var error = BookingValidators.DeleteBookingValidator(user, booking, hotel.OwnerId);

            if (error != null)
            {
                return error;
            }

            context.Bookings.Remove(booking);
            context.SaveChanges();

            return new OkObjectResult("Booking has been deleted.");
        }

        public Booking? GetById(int? id)
        {
            if (id == null)
            {
                return null;
            }

            return (from booking in context.Bookings
                    where id == booking.Id
                    select booking)
                   .FirstOrDefault();
        }

        public IActionResult GetDTOById(int id)
        {
            var user = userStore.GetCurrentUser();
            var booking = GetById(id);
            var room = roomStore.GetById(booking?.RoomId);
            var hotel = hotelStore.GetById(room?.HotelId);

            var error = BookingValidators.GetBookingByIdValidator(user, booking, room, hotel);

            if (error != null)
            {
                return error;
            }

            return new OkObjectResult((from bkng in context.Bookings
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
                   .FirstOrDefault());
        }

        public IEnumerable<Booking> All()
        {
            return from booking in context.Bookings
                   select booking;
        }

        public IActionResult GetBookings(int roomId)
        {
            var user = userStore.GetCurrentUser();
            var room = roomStore.GetById(roomId);
            var hotel = hotelStore.GetById(room?.HotelId);

            var error = BookingValidators.GetBookingsValidator(user, room);

            if (error != null)
            {
                return error;
            }

            return new OkObjectResult(from booking in context.Bookings
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
                                      });
        }
    }
}
