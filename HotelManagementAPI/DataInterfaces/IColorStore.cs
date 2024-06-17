using Microsoft.AspNetCore.Mvc;

namespace HotelManagementAPI.DataInterfaces
{
    public interface IColorStore
    {
        Task<IActionResult> GetColors();
        Task<string?> GetColorById(int id);
    }
}