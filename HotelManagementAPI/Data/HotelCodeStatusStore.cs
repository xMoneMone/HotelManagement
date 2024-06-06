using HotelManagementAPI.DataInterfaces;
using HotelManagementAPI.Models;

namespace HotelManagementAPI.Data
{
    public class HotelCodeStatusStore(HotelManagementContext context) : IHotelCodeStatusStore
    {
        private readonly HotelManagementContext context = context;

        public HotelCodeStatus? GetById(int id)
        {
            return (from status in context.HotelCodeStatuses
                    where id == status.Id
                    select status)
                   .FirstOrDefault();
        }
    }
}
