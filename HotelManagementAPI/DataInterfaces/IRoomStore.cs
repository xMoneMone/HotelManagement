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
        Task<IActionResult> Edit(int roomId, int hotelId, RoomCreateDTO roomDTO);
        Task<Room?> GetById(int? id);
        Task<IActionResult> GetDTOById(int id);
        Task<bool> RoomIsFree(int? roomId, DateTime start, DateTime end);
        Task<IActionResult> GetRooms(int hotelId, DateTime start, DateTime end, bool orderByAvailability = false);
    }
}