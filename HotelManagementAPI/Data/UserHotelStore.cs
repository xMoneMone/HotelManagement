using HotelManagementAPI.Models;

namespace HotelManagementAPI.Data
{
    public class UserHotelStore : DataStore
    {
        public static void Add(HotelCode code)
        {
            context.Add(new UsersHotel
             {
                HotelId = code.HotelId,
                UserId = code.UserId
            });
        }
    }
}
