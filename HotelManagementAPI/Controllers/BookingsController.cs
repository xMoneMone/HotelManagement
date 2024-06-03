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
    }
}
