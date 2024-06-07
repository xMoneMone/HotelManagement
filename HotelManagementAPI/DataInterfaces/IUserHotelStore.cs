using HotelManagementAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementAPI.DataInterfaces
{
    public interface IUserHotelStore
    {
        void Add(HotelCode code);
        Task<IActionResult> Delete(int hotelId, int employeeId);
        Task<UsersHotel?> GetByHotelEmployee(int? hotelId, int? employeeId);
    }
}