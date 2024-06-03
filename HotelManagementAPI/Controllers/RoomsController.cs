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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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

        [HttpPost, Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult CreateRoom([FromBody] RoomCreateDTO roomDTO)
        {
            var user = JwtDecoder.GetUser(User.Claims, UserStore.context);

            var error = RoomValidators.CreateRoomValidator(user, roomDTO);

            if (error != null)
            {
                return error;
            }

            var room = new Room
            {
                RoomNumber = roomDTO.RoomNumber,
                PricePerNight = roomDTO.PricePerNight,
                Notes = roomDTO.Notes,
                HotelId = roomDTO.HotelId
            };

            RoomStore.context.Rooms.Add(room);
            RoomStore.context.SaveChanges();

            return Ok("Room created successfully.");
        }
    }
}
