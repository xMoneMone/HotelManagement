using HotelManagementAPI.Data;
using HotelManagementAPI.Models;
using HotelManagementAPI.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace HotelManagementAPI.Util
{
    public class RoomValidators
    {
        public static IActionResult? GetRoomsValidator(User user, Hotel? hotel, int[] employeesAtHotel)
        {
            if (hotel == null)
            {
                return new NotFoundObjectResult("Hotel does not exist.");
            }

            if (hotel.OwnerId != user.Id && Validators.EmployeeWorksAtHotel(user.Id, employeesAtHotel))
            {
                return new UnauthorizedObjectResult("You do not have permission to see this resource.");
            }

            return null;
        }

        public static IActionResult? CreateRoomValidator(User user, RoomCreateDTO roomDTO, Hotel? hotel)
        {
            if (hotel == null)
            {
                return new NotFoundObjectResult("Hotel does not exist.");
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

        public static IActionResult? EditRoomValidator(User user, RoomCreateDTO roomDTO, Room? room, Hotel? hotel)
        {
            if (room == null)
            {
                return new NotFoundObjectResult("Room does not exist.");
            }

            return CreateRoomValidator(user, roomDTO, hotel);
        }

        public static IActionResult? GetRoomByIdValidator(User user, Room? room, Hotel? hotel, int[] employeesAtHotel)
        {
            if (room == null)
            {
                return new NotFoundObjectResult("Room does not exist.");
            }

            if (hotel.OwnerId != user.Id && Validators.EmployeeWorksAtHotel(user.Id, employeesAtHotel))
            {
                return new UnauthorizedObjectResult("You do not have permission to see this resource.");
            }

            return null;
        }

        public static IActionResult? DeleteRoomValidator(User user, Room? room, Hotel? hotel)
        {
            if (room == null)
            {
                return new NotFoundObjectResult("Room does not exist.");
            }

            if (hotel.OwnerId != user.Id)
            {
                return new UnauthorizedObjectResult("You do not have permission to delete this resource.");
            }

            return null;
        }
    }
}
