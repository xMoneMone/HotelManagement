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
            var user = JwtDecoder.GetUser(User.Claims, UserStore.context);
            var hotel = HotelStore.context.Hotels.FirstOrDefault(x => x.Id == hotelId);

            var error = RoomValidators.GetRoomsValidator(user, hotel);

            if (error != null)
            {
                return error;
            }

            return Ok(RoomStore.GetRooms(hotel));
        }

        [HttpPost, Authorize(Roles = "Owner")]
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

        [HttpGet("{roomId:int}"), Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult GetRoomById(int roomId)
        {
            var user = JwtDecoder.GetUser(User.Claims, UserStore.context);

            var error = RoomValidators.GetRoomByIdValidator(user, roomId);

            if (error != null)
            {
                return error;
            }

            var room = RoomStore.context.Rooms.FirstOrDefault(x => x.Id == roomId);
            var hotel = HotelStore.context.Hotels.FirstOrDefault(x => x.Id == room.HotelId);
            return Ok(new RoomDTO
            {
                Id = room.Id,
                RoomNumber = room.RoomNumber,
                PricePerNight = room.PricePerNight,
                Notes = room.Notes,
                CurrencyFormat = RoomStore.context.Currencies.FirstOrDefault(x => x.Id == hotel.CurrencyId).FormattingString
            });

        }

        [HttpDelete("{roomId:int}"), Authorize(Roles = "Owner")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult DeleteRoom(int roomId)
        {
            var user = JwtDecoder.GetUser(User.Claims, UserStore.context);

            var error = RoomValidators.DeleteRoomValidator(user, roomId);

            if (error != null)
            {
                return error;
            }

            var room = RoomStore.context.Rooms.FirstOrDefault(x => x.Id == roomId);
            RoomStore.context.Rooms.Remove(room);
            RoomStore.context.SaveChanges();

            return Ok("Room has been deleted.");
        }
    }
}
