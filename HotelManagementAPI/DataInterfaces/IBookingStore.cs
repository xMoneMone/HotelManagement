using HotelManagementAPI.Models;
using HotelManagementAPI.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementAPI.DataInterfaces
{
    public interface IBookingStore
    {
        IActionResult Add(BookingCreateDTO bookingDTO, int roomId);
        IEnumerable<Booking> All();
        IActionResult Delete(int id);
        IActionResult Edit(int id, BookingCreateDTO bookingDTO);
        IActionResult GetBookings(int roomId);
        Booking? GetById(int? id);
        IActionResult GetDTOById(int id);
    }
}