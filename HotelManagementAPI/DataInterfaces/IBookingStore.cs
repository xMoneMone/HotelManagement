using HotelManagementAPI.Models;
using HotelManagementAPI.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementAPI.DataInterfaces
{
    public interface IBookingStore
    {
        Task<IActionResult> Add(BookingCreateDTO bookingDTO, int roomId);
        Task<IEnumerable<Booking>> All();
        Task<IActionResult> Delete(int id);
        Task<IActionResult> Edit(int id, BookingCreateDTO bookingDTO);
        Task<IActionResult> GetBookings(int roomId);
        Task<Booking?> GetById(int? id);
        Task<IActionResult> GetDTOById(int id);
    }
}