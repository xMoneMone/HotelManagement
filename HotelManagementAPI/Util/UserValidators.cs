using HotelManagementAPI.Models.DTO;
using HotelManagementAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementAPI.Util
{
    public class UserValidators
    {
        public static IActionResult? CreateUserValidator(UserCreateDTO userDTO)
        {
            if (userDTO == null)
            {
                return new BadRequestObjectResult(userDTO);
            }

            return null;
        }

        public static IActionResult? LoginValidator(UserLoginDTO userDTO, User? user)
        {
            if (user == null)
            {
                return new UnauthorizedObjectResult("Wrong email or password.");
            }

            if (!BCrypt.Net.BCrypt.Verify(userDTO.Password, user.Password))
            {
                return new UnauthorizedObjectResult("Wrong email or password.");
            }

            return null;
        }

        public static IActionResult? EditUserValidator(UserEditDTO userDTO)
        {
            return null;
        }
    }
}
