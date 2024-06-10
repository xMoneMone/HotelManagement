﻿using HotelManagementAPI.DataInterfaces;
using HotelManagementAPI.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementAPI.Controllers
{
    [Route(""), Authorize]
    [ApiController]
    public class BookingsController(IBookingStore bookingStore) : ControllerBase
    {
        private readonly IBookingStore bookingStore = bookingStore;

        [HttpGet("hotels/{hotelId}/rooms/{roomId:int}/bookings"), Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetBookings(int roomId, int hotelId)
        {
            return await bookingStore.GetBookings(roomId);
        }

        [HttpPost("hotels/{hotelId:int}/rooms/{roomId:int}/bookings"), Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateBooking([FromBody] BookingCreateDTO bookingDTO, int roomId, int hotelId)
        {
            return await bookingStore.Add(bookingDTO, roomId);
        }

        [HttpPatch("hotels/{hotelId:int}/rooms/{roomId:int}/bookings/{bookingId:int}"), Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> EditBooking([FromBody] BookingCreateDTO bookingDTO, int bookingId, int roomId, int hotelId)
        {
            return await bookingStore.Edit(bookingId, bookingDTO);
        }

        [HttpGet("hotels/{hotelId:int}/rooms/{roomId:int}/bookings/{bookingId:int}"), Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetBookingById(int bookingId, int roomId, int hotelId)
        {
            return await bookingStore.GetDTOById(bookingId);
        }

        [HttpDelete("hotels/{hotelId:int}/rooms/{roomId:int}/bookings/{bookingId:int}"), Authorize(Roles = "Owner")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DeleteBooking(int bookingId, int roomId, int hotelId)
        {
            return await bookingStore.Delete(bookingId);
        }

    }
}
