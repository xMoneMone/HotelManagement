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
    }
}
