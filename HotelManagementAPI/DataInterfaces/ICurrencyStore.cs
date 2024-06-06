using HotelManagementAPI.Models;

namespace HotelManagementAPI.DataInterfaces
{
    public interface ICurrencyStore
    {
        Currency? GetById(int id);
    }
}