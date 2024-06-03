using HotelManagementAPI.Data;
using HotelManagementAPI.Models;
using HotelManagementAPI.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace HotelManagementAPI.Util
{
    public class RoomValidators
    {
        public static IActionResult? GetRoomsValidator(User user, Hotel hotel)
        {
            if (hotel == null)
            {
                return new BadRequestObjectResult("Hotel does not exist.");
            }

            if (hotel.OwnerId != user.Id && Validators.EmployeeWorksAtHotel(hotel.Id, user.Id))
            {
                return new UnauthorizedObjectResult("You do not have permission to see this resource.");
            }

            return null;
        }

        public static IActionResult? CreateRoomValidator(User user, RoomCreateDTO roomDTO)
        {
            var hotel = HotelStore.context.Hotels.FirstOrDefault(x => x.Id == roomDTO.HotelId);

            if (hotel == null)
            {
                return new BadRequestObjectResult("Hotel does not exist.");
            }

            if (hotel.OwnerId != user.Id)
            {
                return new UnauthorizedObjectResult("You are not the owner of this hotel.");
            }

            if (roomDTO.RoomNumber.Length <= 0 || roomDTO.RoomNumber.Length > 50)
            {
                return new BadRequestObjectResult("Room number must be below 50 characters.");
            }

            if (roomDTO.PricePerNight < 0)
            {
                return new BadRequestObjectResult("Room price cannot be a negative number.");
            }

            if (roomDTO.Notes != null && roomDTO.Notes.Length > 300)
            {
                return new BadRequestObjectResult("Room notes must not exceed 300 characters.");
            }

            return null;
        }

        public static IActionResult? EditRoomValidator(User user, RoomCreateDTO roomDTO, Room? room)
        {
            if (room == null)
            {
                return new BadRequestObjectResult("Room does not exist.");
            }

            return CreateRoomValidator(user, roomDTO);
        }

        public static IActionResult? GetRoomByIdValidator(User user, int roomId)
        {
            var room = RoomStore.context.Rooms.FirstOrDefault(x => x.Id == roomId);
            var hotel = HotelStore.context.Hotels.FirstOrDefault(x => x.Id == room.HotelId);

            if (room == null)
            {
                return new BadRequestObjectResult("Room does not exist.");
            }

            if (hotel.OwnerId != user.Id && Validators.EmployeeWorksAtHotel(hotel.Id, user.Id))
            {
                return new UnauthorizedObjectResult("You do not have permission to see this resource.");
            }

            return null;
        }

        public static IActionResult? DeleteRoomValidator(User user, int roomId)
        {
            var room = RoomStore.context.Rooms.FirstOrDefault(x => x.Id == roomId);
            var hotel = HotelStore.context.Hotels.FirstOrDefault(x => x.Id == room.HotelId);

            if (room == null)
            {
                return new BadRequestObjectResult("Room does not exist.");
            }

            if (hotel.OwnerId != user.Id)
            {
                return new UnauthorizedObjectResult("You do not have permission to delete this resource.");
            }

            return null;
        }
    }
}
