using HotelManagementAPI.Data;
using HotelManagementAPI.Models;
using HotelManagementAPI.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementAPI.Util
{
    public class InvitationValidators
    {
        public static IActionResult? InviteEmployeeValidator(User user, string employeeEmail, int hotelId)
        {
            var employee = UserStore.GetByEmail(employeeEmail);
            var hotel = HotelStore.GetById(hotelId);

            if (employee == null || hotel == null)
            {
                return new BadRequestObjectResult("User or hotel does not exist.");
            }

            var userHotelConnection = HotelCodeStore.GetByEmployeeHotel(employeeEmail, hotelId);

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

        public static IActionResult? RespondToInvitationValidator(User user, string codeId)
        {
            var code = HotelCodeStore.GetById(codeId);

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

        public static IActionResult? DeleteInviteValidator(User user, string codeId)
        {
            var code = HotelCodeStore.GetById(codeId);

            if (code == null)
            {
                return new BadRequestObjectResult("Code does not exist.");
            }

            Hotel hotel = HotelStore.GetById(code.HotelId);
            var hotelOwner = UserStore.GetById(hotel.OwnerId);

            if (user.Id != hotelOwner.Id)
            {
                return new UnauthorizedObjectResult("You're not the creator of this invite.");
            }

            return null;
        }
    }
}
