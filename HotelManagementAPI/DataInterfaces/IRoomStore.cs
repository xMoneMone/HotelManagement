using HotelManagementAPI.Models;
using HotelManagementAPI.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementAPI.DataInterfaces
{
    public interface IRoomStore
    {
        IActionResult Add(RoomCreateDTO roomDTO, int hotelId);
        IEnumerable<Room> All();
        IActionResult Delete(int id);
        IActionResult Edit(int id, RoomCreateDTO roomDTO);
        Room? GetById(int id);
        IActionResult GetDTOById(int id);
        IActionResult GetRooms(int hotelId);
    }
}