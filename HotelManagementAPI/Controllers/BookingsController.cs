using HotelManagementAPI.DataInterfaces;
using HotelManagementAPI.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementAPI.Controllers
{
    [Route("bookings"), Authorize]
    [ApiController]
    public class BookingsController(IBookingStore bookingStore) : ControllerBase
    {
        private readonly IBookingStore bookingStore = bookingStore;

        [HttpGet("all-bookings/{roomId:int}"), Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult GetBookings(int roomId)
        {
            return bookingStore.GetBookings(roomId);
        }

        [HttpPost("{roomId:int}"), Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult CreateBooking([FromBody] BookingCreateDTO bookingDTO, int roomId)
        {
            return bookingStore.Add(bookingDTO, roomId);
        }

        [HttpPut("{bookingId:int}"), Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult EditBooking([FromBody] BookingCreateDTO bookingDTO, int bookingId)
        {
            return bookingStore.Edit(bookingId, bookingDTO);
        }

        [HttpGet("{bookingId:int}"), Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult GetBookingById(int bookingId)
        {
            return bookingStore.GetDTOById(bookingId);
        }

        [HttpDelete("{bookingId:int}"), Authorize(Roles = "Owner")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult DeleteBooking(int bookingId)
        {
            return bookingStore.Delete(bookingId);
        }

    }
}
