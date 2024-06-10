using HotelManagementAPI.DataInterfaces;
using HotelManagementAPI.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementAPI.Controllers
{
    [Route("hotels/rooms"), Authorize]
    [ApiController]
    public class BookingsController(IBookingStore bookingStore) : ControllerBase
    {
        private readonly IBookingStore bookingStore = bookingStore;

        [HttpGet("{roomId:int}/bookings"), Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetBookings(int roomId)
        {
            return await bookingStore.GetBookings(roomId);
        }

        [HttpPost("{roomId:int}/bookings"), Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> CreateBooking([FromBody] BookingCreateDTO bookingDTO, int roomId)
        {
            return await bookingStore.Add(bookingDTO, roomId);
        }

        [HttpPatch("bookings/{bookingId:int}"), Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> EditBooking([FromBody] BookingCreateDTO bookingDTO, int bookingId)
        {
            return await bookingStore.Edit(bookingId, bookingDTO);
        }

        [HttpGet("bookings/{bookingId:int}"), Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetBookingById(int bookingId)
        {
            return await bookingStore.GetDTOById(bookingId);
        }

        [HttpDelete("bookings/{bookingId:int}"), Authorize(Roles = "Owner")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DeleteBooking(int bookingId)
        {
            return await bookingStore.Delete(bookingId);
        }

    }
}
