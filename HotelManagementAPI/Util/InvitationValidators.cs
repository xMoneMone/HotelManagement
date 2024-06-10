using HotelManagementAPI.Data;
using HotelManagementAPI.Models;
using HotelManagementAPI.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementAPI.Util
{
    public class InvitationValidators
    {
        public static IActionResult? InviteEmployeeValidator(User user, User employee, Hotel hotel, UsersHotel userHotelConnection)
        {
            if (employee == null || hotel == null)
            {
                return new NotFoundObjectResult("User or hotel does not exist.");
            }

            if (userHotelConnection != null)
            {
                return new BadRequestObjectResult("User is already an employee of this hotel.");
            }

            if (user.Id != hotel.OwnerId)
            {
                return new BadRequestObjectResult("You are not the owner of this hotel.");
            }

            if (user == employee)
            {
                return new BadRequestObjectResult("Can't invite yourself to your hotel.");
            }

            return null;
        }

        public static IActionResult? RespondToInvitationValidator(User user, HotelCode? code)
        {
            if (code == null)
            {
                return new NotFoundObjectResult("Code does not exist.");
            }

            if (user.Id != code.UserId)
            {
                return new ObjectResult("You are not the recepient of this invite.") { StatusCode = 403 };
            }

            if (code.StatusId != 1)
            {
                return new BadRequestObjectResult("Code already used.");
            }

            return null;
        }

        public static IActionResult? DeleteInviteValidator(int userId, HotelCode? code, int hotelOwnerId)
        {
            if (code == null)
            {
                return new NotFoundObjectResult("Code does not exist.");
            }

            if (userId != hotelOwnerId)
            {
                return new ObjectResult("You're not the creator of this invite.") { StatusCode = 403 };
            }

            return null;
        }
    }
}
