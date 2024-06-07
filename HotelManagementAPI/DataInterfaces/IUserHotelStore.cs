using HotelManagementAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlTypes;

namespace HotelManagementAPI.DataInterfaces
{
    public interface IUserHotelStore
    {
        Task<bool> Add(HotelCode code);
        Task<IActionResult> Delete(int hotelId, int employeeId);
        Task<UsersHotel?> GetByHotelEmployee(int? hotelId, int? employeeId);
    }
}