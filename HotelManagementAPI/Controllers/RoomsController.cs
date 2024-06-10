using HotelManagementAPI.DataInterfaces;
using HotelManagementAPI.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementAPI.Controllers
{
    [Route("hotels/{hotelId:int}/rooms"), Authorize]
    [ApiController]
    public class RoomsController(IRoomStore roomStore) : ControllerBase
    {
        private readonly IRoomStore roomStore = roomStore;

        [HttpGet(""), Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetRooms(int hotelId)
        {
            return await roomStore.GetRooms(hotelId);
        }

        [HttpPost, Authorize(Roles = "Owner")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CreateRoom([FromBody] RoomCreateDTO roomDTO, int hotelId)
        {
            return await roomStore.Add(roomDTO, hotelId);
        }

        [HttpPatch("{roomId:int}"), Authorize(Roles = "Owner")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> EditRoom([FromBody] RoomCreateDTO roomDTO, int roomId, int hotelId)
        {
            return await roomStore.Edit(roomId, roomDTO);
        }

        [HttpGet("{roomId:int}"), Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetRoomById(int roomId, int hotelId)
        {
            return await roomStore.GetDTOById(roomId);
        }

        [HttpDelete("{roomId:int}"), Authorize(Roles = "Owner")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteRoom(int roomId, int hotelId)
        {
            return await roomStore.Delete(roomId);
        }
    }
}
