using HotelManagementAPI.Data;
using HotelManagementAPI.Models;
using HotelManagementAPI.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementAPI.Util
{
    public class InvitationValidators
    {
        public static IActionResult? InviteEmployeeValidator(User user, User? employee, Hotel? hotel)
        {
            if (employee == null || hotel == null)
            {
                return new BadRequestObjectResult("User or hotel does not exist.");
            }

            var userHotelConnection = HotelStore.context.UsersHotels.FirstOrDefault(x => x.UserId == employee.Id && x.HotelId == hotel.Id);

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

        public static IActionResult? RespondToInvitationValidator(User user, HotelCode code)
        {
            if (code == null)
            {
                return new BadRequestObjectResult("Code does not exist.");
            }

            if (user.Id != code.UserId)
            {
                return new UnauthorizedObjectResult("You are not the recepient of this invite.");
            }

            if (code.StatusId != 1)
            {
                return new BadRequestObjectResult("Code already used.");
            }

            return null;
        }
    }
}
