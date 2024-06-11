using HotelManagementAPI.Models.DTO;
using HotelManagementAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace HotelManagementAPI.Util
{
    public class UserValidators
    {
        public static IActionResult? CreateUserValidator(UserCreateDTO userDTO)
        {
            Regex emailRegex = new(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
            Regex passwordRegex = new(@"(?=^.{8,}$)(?=.*\d)(?=.*[!@#$%^&*]+)(?![.\n])(?=.*[A-Z])(?=.*[a-z]).*$");

            if (userDTO == null)
            {
                return new BadRequestObjectResult(userDTO);
            }

            if (!emailRegex.IsMatch(userDTO.Email))
            {
                return new BadRequestObjectResult("Invalid email.");
            }

            if (!passwordRegex.IsMatch(userDTO.Password))
            {
                return new BadRequestObjectResult("Password must be at least 8 characters and must contain at least one uppercase letter," +
                                                   " lowercase letter, number, and special character.");
            }

            if (userDTO.FirstName.Length <= 0 || userDTO.FirstName.Length > 50)
            {
                return new BadRequestObjectResult("First name number must be below 50 characters.");
            }

            if (userDTO.LastName.Length <= 0 || userDTO.LastName.Length > 50)
            {
                return new BadRequestObjectResult("First name number must be below 50 characters.");
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

            if (userDTO.FirstName.Length <= 0 || userDTO.FirstName.Length > 50)
            {
                return new BadRequestObjectResult("First name number must be below 50 characters.");
            }

            if (userDTO.LastName.Length <= 0 || userDTO.LastName.Length > 50)
            {
                return new BadRequestObjectResult("First name number must be below 50 characters.");
            }

            return null;
        }
    }
}
