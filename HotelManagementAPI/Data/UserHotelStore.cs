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

        public static UsersHotel? GetByHotelEmployee(int hotelId, int employeeId)
        {
            return context.UsersHotels.FirstOrDefault(x => x.UserId == employeeId && x.HotelId == hotelId);
        }
    }
}
