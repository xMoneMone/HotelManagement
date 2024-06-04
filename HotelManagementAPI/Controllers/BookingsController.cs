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
            var user = JwtDecoder.GetUser(User.Claims, DataStore.context);
            var room = RoomStore.GetById(roomId);

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
        public IActionResult CreateBooking([FromBody] BookingCreateDTO bookingDTO, int roomId)
        {
            var user = JwtDecoder.GetUser(User.Claims, DataStore.context);

            var error = BookingValidators.CreateBookingValidator(user, bookingDTO, roomId);

            if (error != null)
            {
                return error;
            }

            BookingStore.Add(bookingDTO, roomId);

            return Ok("Booking created successfully.");
        }

        [HttpPut("{bookingId:int}"), Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult EditBooking([FromBody] BookingCreateDTO bookingDTO, int bookingId)
        {
            var user = JwtDecoder.GetUser(User.Claims, DataStore.context);

            var error = BookingValidators.EditBookingValidator(user, bookingDTO, bookingId);

            if (error != null)
            {
                return error;
            }

            BookingStore.Edit(bookingId, bookingDTO);
            return Ok("Booking edited successfully.");
        }

        [HttpGet("{bookingId:int}"), Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult GetBookingById(int bookingId)
        {
            var user = JwtDecoder.GetUser(User.Claims, DataStore.context);
            var booking = BookingStore.GetById(bookingId);

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
            var user = JwtDecoder.GetUser(User.Claims, DataStore.context);

            var error = BookingValidators.DeleteBookingValidator(user, bookingId);

            if (error != null)
            {
                return error;
            }

            BookingStore.Delete(bookingId);
            return Ok("Booking has been deleted.");
        }

    }
}
