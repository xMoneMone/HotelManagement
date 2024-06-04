using HotelManagementAPI.Models;
using HotelManagementAPI.Models.DTO;

namespace HotelManagementAPI.Data
{
    public class UserStore
    {
        public static HotelManagementContext context = new HotelManagementContext();
        public static User? GetByEmail(string email)
        {
            return (from user in context.Users
                    where email == user.Email
                    select user)
                   .FirstOrDefault();
        }

    }
}
