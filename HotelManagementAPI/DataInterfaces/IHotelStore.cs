using HotelManagementAPI.Models;
using HotelManagementAPI.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementAPI.DataInterfaces
{
    public interface IHotelStore
    {
        IActionResult Add(HotelCreateDTO hotelDTO);
        IEnumerable<Hotel> All();
        IActionResult Delete(int id);
        IActionResult Edit(int id, HotelCreateDTO hotelDTO);
        Hotel? GetById(int? id);
        int[] GetHotelEmployeesIds(int hotelId);
        IEnumerable<HotelDTO> GetUserHotels();
    }
}