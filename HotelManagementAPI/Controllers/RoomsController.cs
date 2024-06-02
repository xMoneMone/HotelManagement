using HotelManagementAPI.Data;
using HotelManagementAPI.Models;
using HotelManagementAPI.Models.DTO;
using HotelManagementAPI.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementAPI.Controllers
{
    [Route("rooms"), Authorize]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        [HttpGet("{hotelId:int}"), Authorize]
        public IActionResult GetRooms(int hotelId)
        {
            var user = JwtDecoder.GetUser(User.Claims, UserStore.context);

            var error = RoomValidators.GetRoomsValidator(user, hotelId);

            if (error != null)
            {
                return error;
            }

            return Ok(RoomStore.GetRooms(hotelId));
        }
    }
}
