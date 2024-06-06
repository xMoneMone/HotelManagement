using HotelManagementAPI.DataInterfaces;
using HotelManagementAPI.Models;
using HotelManagementAPI.Models.DTO;
using HotelManagementAPI.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementAPI.Controllers
{
    [Route("rooms"), Authorize]
    [ApiController]
    public class RoomsController(IUserStore userStore, IRoomStore roomStore) : ControllerBase
    {
        private readonly IUserStore userStore = userStore;
        private readonly IRoomStore roomStore = roomStore;

        [HttpGet("all-rooms/{hotelId:int}"), Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult GetRooms(int hotelId)
        {
            return roomStore.GetRooms(hotelId);
        }

        [HttpPost("{hotelId:int}"), Authorize(Roles = "Owner")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult CreateRoom([FromBody] RoomCreateDTO roomDTO, int hotelId)
        {
            return roomStore.Add(roomDTO, hotelId);
        }

        [HttpPut("{roomId:int}"), Authorize(Roles = "Owner")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult EditRoom([FromBody] RoomCreateDTO roomDTO, int roomId)
        {
            return roomStore.Edit(roomId, roomDTO);
        }

        [HttpGet("{roomId:int}"), Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult GetRoomById(int roomId)
        {
            return roomStore.GetDTOById(roomId);
        }

        [HttpDelete("{roomId:int}"), Authorize(Roles = "Owner")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult DeleteRoom(int roomId)
        {
            return roomStore.Delete(roomId);
        }
    }
}
