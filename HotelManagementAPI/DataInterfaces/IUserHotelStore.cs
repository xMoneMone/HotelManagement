using HotelManagementAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementAPI.DataInterfaces
{
    public interface IUserHotelStore
    {
        void Add(HotelCode code);
        IActionResult Delete(int hotelId, int employeeId);
        UsersHotel? GetByHotelEmployee(int hotelId, int employeeId);
    }
}