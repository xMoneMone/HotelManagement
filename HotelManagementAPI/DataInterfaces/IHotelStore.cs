using HotelManagementAPI.Models;
using HotelManagementAPI.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementAPI.DataInterfaces
{
    public interface IHotelStore
    {
        Task<IActionResult> Add(HotelCreateDTO hotelDTO);
        Task<IEnumerable<Hotel>> All();
        Task<IActionResult> Delete(int id);
        Task<IActionResult> Edit(int id, HotelCreateDTO hotelDTO);
        Task<Hotel?> GetById(int? id);
        Task<IActionResult> GetDTOById(int id);
        Task<int[]> GetHotelEmployeesIds(int? hotelId);
        Task<IActionResult> GetUserHotels();
    }
}