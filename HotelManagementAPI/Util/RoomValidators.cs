using HotelManagementAPI.Data;
using HotelManagementAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace HotelManagementAPI.Util
{
    public class RoomValidators
    {
        public static IActionResult? GetRoomsValidator(User user, int hotelId)
        {
            var hotel = HotelStore.context.Hotels.FirstOrDefault(x => x.Id == hotelId);
            var employeesAtHotel = HotelStore.GetHotelEmployeesIds(hotelId);

            if (hotel ==  null)
            {
                return new BadRequestObjectResult("Hotel does not exist.");
            }

            if (hotel.OwnerId != user.Id && !employeesAtHotel.Contains(user.Id))
            {
                return new UnauthorizedObjectResult("You do not have permission to see this resource.");
            }

            return null;
        }
    }
}
