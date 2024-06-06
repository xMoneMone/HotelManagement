using HotelManagementAPI.Models;

namespace HotelManagementAPI.DataInterfaces
{
    public interface IAccountTypeStore
    {
        AccountType? GetById(int id);
    }
}