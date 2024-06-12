using HotelManagementAPI.Data;
using HotelManagementAPI.Models;
using HotelManagementAPI.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HotelManagementAPI.Util
{
    public class HotelValidators
    {
        public static IActionResult? RemoveHotelEmployeeValidator(User user, User? employee, Hotel? hotel, UsersHotel? userHotelConnection)
        {
            if (hotel == null || employee == null)
            {
                return new NotFoundObjectResult("Hotel or employee does not exist.");
            }

            if (user.Id != hotel.OwnerId)
            {
                return new ObjectResult("You are not authorized to perform this action.") { StatusCode = 403 };
            }

            if (userHotelConnection == null)
            {
                return new BadRequestObjectResult("User does not work in this hotel.");
            }

            return null;
        }

        public static IActionResult? CreateHotelValidator(HotelCreateDTO hotelDTO)
        {
            if (hotelDTO == null)
            {
                return new BadRequestObjectResult(hotelDTO);
            }

            if (hotelDTO.Name.Length == 0 || hotelDTO.Name.Length > 100)
            {
                return new BadRequestObjectResult("Hotel name must be under 100 characters.");
            }

            if (hotelDTO.DownPaymentPercentage < 0)
            {
                return new BadRequestObjectResult("Down payment percentage cannot be a negative number.");
            }

            return null;
        }

        public static IActionResult? GetHotelCurrencyValidator(Hotel? hotel)
        {
            if (hotel == null)
            {
                return new NotFoundObjectResult("Hotel does not exist.");
            }

            return null;
        }

        public static IActionResult? GetByIdValidator(Hotel? hotel, User user)
        {
            if (hotel == null)
            {
                return new NotFoundObjectResult("Hotel does not exist.");
            }

            if (hotel.OwnerId != user.Id)
            {
                return new ObjectResult("You do not have permission to view this resource.") { StatusCode = 403 };
            }

            return null;
        }

        public static IActionResult? EditHotelValidator(HotelCreateDTO hotelDTO, Hotel? hotel, User user)
        {
            if (hotelDTO == null)
            {
                return new NotFoundObjectResult("Empty hotel.");
            }
            if (hotel == null)
            {
                return new NotFoundObjectResult("Hotel does not exist.");
            }

            if (hotel.OwnerId != user.Id)
            {
                return new ObjectResult("You are not the owner of this hotel.") { StatusCode = 403 };
            }

            return CreateHotelValidator(hotelDTO);
        }

        public static IActionResult? DeleteHotelValidator(Hotel? hotel, User user)
        {
            if (hotel == null)
            {
                return new NotFoundObjectResult("Hotel does not exist.");
            }

            if (hotel.OwnerId != user.Id)
            {
                return new ObjectResult("You are not the owner of this hotel.") { StatusCode = 403 };
            }

            return null;
        }
    }

}
