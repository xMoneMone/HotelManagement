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
        [HttpGet("all-rooms/{hotelId:int}"), Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult GetRooms(int hotelId)
        {
            var user = JwtDecoder.GetUser(User.Claims, DataStore.context);

            var error = RoomValidators.GetRoomsValidator(user, hotelId);

            if (error != null)
            {
                return error;
            }

            return Ok(RoomStore.GetRooms(hotelId));
        }

        [HttpPost("{hotelId:int}"), Authorize(Roles = "Owner")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult CreateRoom([FromBody] RoomCreateDTO roomDTO, int hotelId)
        {
            var user = JwtDecoder.GetUser(User.Claims, DataStore.context);

            var error = RoomValidators.CreateRoomValidator(user, roomDTO);

            if (error != null)
            {
                return error;
            }

            RoomStore.Add(roomDTO, hotelId, user);
            return Ok("Room created successfully.");
        }

        [HttpPut("{roomId:int}"), Authorize(Roles = "Owner")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult EditRoom([FromBody] RoomCreateDTO roomDTO, int roomId)
        {
            var user = JwtDecoder.GetUser(User.Claims, DataStore.context);

            var error = RoomValidators.EditRoomValidator(user, roomDTO, roomId);

            if (error != null)
            {
                return error;
            }

            RoomStore.Edit(roomId, roomDTO);
            return Ok("Room created successfully.");
        }

        [HttpGet("{roomId:int}"), Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult GetRoomById(int roomId)
        {
            var user = JwtDecoder.GetUser(User.Claims, DataStore.context);

            var error = RoomValidators.GetRoomByIdValidator(user, roomId);

            if (error != null)
            {
                return error;
            }

            return Ok(RoomStore.GetDTOById(roomId));

        }

        [HttpDelete("{roomId:int}"), Authorize(Roles = "Owner")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult DeleteRoom(int roomId)
        {
            var user = JwtDecoder.GetUser(User.Claims, DataStore.context);

            var error = RoomValidators.DeleteRoomValidator(user, roomId);

            if (error != null)
            {
                return error;
            }

            RoomStore.Delete(roomId);

            return Ok("Room has been deleted.");
        }
    }
}
