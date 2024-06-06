using HotelManagementAPI.Models;

namespace HotelManagementAPI.Data
{
    public interface IHotelCodeStatusStore
    {
        HotelCodeStatus? GetById(int id);
    }
}