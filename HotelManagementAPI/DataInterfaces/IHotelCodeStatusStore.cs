using HotelManagementAPI.Models;

namespace HotelManagementAPI.Data
{
    public interface IHotelCodeStatusesStore
    {
        HotelCodeStatus? GetById(int id);
    }
}