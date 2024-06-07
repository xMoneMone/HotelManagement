using HotelManagementAPI.Models;
using HotelManagementAPI.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementAPI.DataInterfaces
{
    public interface IRoomStore
    {
        Task<IActionResult> Add(RoomCreateDTO roomDTO, int hotelId);
        Task<IEnumerable<Room>> All();
        Task<IActionResult> Delete(int id);
        Task<IActionResult> Edit(int id, RoomCreateDTO roomDTO);
        Task<Room?> GetById(int? id);
        Task<IActionResult> GetDTOById(int id);
        Task<IActionResult> GetRooms(int hotelId);
    }
}