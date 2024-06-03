using HotelManagementAPI.Data;
using HotelManagementAPI.Models;
using HotelManagementAPI.Models.DTO;
using HotelManagementAPI.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementAPI.Controllers
{
    [Route("bookings"), Authorize]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        [HttpGet("all-bookings/{roomId:int}"), Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult GetBookings(int roomId)
        {
            var user = JwtDecoder.GetUser(User.Claims, UserStore.context);
            var room = RoomStore.context.Rooms.FirstOrDefault(x => x.Id == roomId);

            var error = BookingValidators.GetBookingsValidator(user, room);

            if (error != null)
            {
                return error;
            }

            return Ok(BookingStore.GetBookings(room));
        }

        [HttpPost("{roomId:int}"), Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult CreateBooking([FromBody] CreateBookingDTO bookingDTO, int roomId)
        {
            var user = JwtDecoder.GetUser(User.Claims, UserStore.context);

            var error = BookingValidators.CreateBookingValidator(user, bookingDTO, roomId);

            if (error != null)
            {
                return error;
            }

            var booking = new Booking
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
            };

            BookingStore.context.Bookings.Add(booking);
            BookingStore.context.SaveChanges();

            return Ok("Booking created successfully.");
        }

        [HttpPut("{bookingId:int}"), Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult EditBooking([FromBody] CreateBookingDTO bookingDTO, int bookingId)
        {
            var user = JwtDecoder.GetUser(User.Claims, UserStore.context);
            var booking = BookingStore.context.Bookings.FirstOrDefault(x => x.Id == bookingId);

            var error = BookingValidators.EditBookingValidator(user, bookingDTO, booking);

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

            BookingStore.context.SaveChanges();

            return Ok("Booking edited successfully.");
        }

        [HttpGet("{bookingId:int}"), Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult GetBookingById(int bookingId)
        {
            var user = JwtDecoder.GetUser(User.Claims, UserStore.context);
            var booking = BookingStore.context.Bookings.FirstOrDefault(x => x.Id == bookingId);

            var error = BookingValidators.GetBookingByIdValidator(user, bookingId);

            if (error != null)
            {
                return error;
            }

            return Ok(new BookingDTO
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
                RoomId = booking.RoomId
            });

        }

        [HttpDelete("{bookingId:int}"), Authorize(Roles = "Owner")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult DeleteBooking(int bookingId)
        {
            var user = JwtDecoder.GetUser(User.Claims, UserStore.context);

            var error = BookingValidators.DeleteBookingValidator(user, bookingId);

            if (error != null)
            {
                return error;
            }

            var booking = BookingStore.context.Bookings.FirstOrDefault(x => x.Id == bookingId);
            BookingStore.context.Bookings.Remove(booking);
            BookingStore.context.SaveChanges();

            return Ok("Booking has been deleted.");
        }

    }
}
