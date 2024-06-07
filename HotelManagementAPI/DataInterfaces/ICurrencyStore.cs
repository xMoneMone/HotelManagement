using HotelManagementAPI.Models;

namespace HotelManagementAPI.DataInterfaces
{
    public interface ICurrencyStore
    {
        Task<Currency?> GetById(int? id);
    }
}