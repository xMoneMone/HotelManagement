using HotelManagementAPI.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementAPI.DataInterfaces
{
    public interface IBedStore
    {
        Task<List<BedDTO>> GetRoomBeds(int roomId);
        Task<IActionResult> GetBeds();
    }
}