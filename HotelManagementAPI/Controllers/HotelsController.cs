﻿using HotelManagementAPI.Data;
using HotelManagementAPI.Models;
using HotelManagementAPI.Models.DTO;
using HotelManagementAPI.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementAPI.Controllers
{
    [Route("hotels"), Authorize]
    [ApiController]
    public class HotelsController : ControllerBase
    {
        [HttpGet, Authorize]
        public IEnumerable<HotelDTO> GetHotels()
        {
            var user = JwtDecoder.GetUser(User.Claims, UserStore.context);
            return HotelStore.GetUserHotels(user);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpDelete("{hotelId}/employees/{employeeId}"), Authorize]
        public IActionResult RemoveHotelEmployee(int hotelId, int employeeId)
        {
            var user = JwtDecoder.GetUser(User.Claims, UserStore.context);
            var hotel = HotelStore.context.Hotels.FirstOrDefault(x => x.Id == hotelId);
            var employee = HotelStore.context.Users.FirstOrDefault(x => x.Id == employeeId);

            if (hotel == null || employee == null)
            {
                return BadRequest("Hotel or employee does not exist.");
            }

            if (user.Id != hotel.OwnerId)
            {
                return Unauthorized("You are not authorized to perform this action.");
            }

            var userHotelConnection = HotelStore.context.UsersHotels.FirstOrDefault(x => x.UserId == employee.Id && x.HotelId == hotel.Id);

            if (userHotelConnection == null)
            {
                return BadRequest("User does not work in this hotel.");
            }

            HotelStore.context.UsersHotels.Remove(userHotelConnection);
            HotelStore.context.SaveChanges();
            return Ok("Employee removed from hotel.");
        }

        [HttpPost, Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public ActionResult<HotelCreateDTO> CreateHotel([FromBody] HotelCreateDTO hotelDTO)
        {
            var user = JwtDecoder.GetUser(User.Claims, UserStore.context);

            if (hotelDTO == null)
            {
                return BadRequest(hotelDTO);
            }

            if (hotelDTO.Name.Length == 0 || hotelDTO.Name.Length > 100)
            {
                return BadRequest("Hotel name must be under 100 characters.");
            }

            UserStore.context.Hotels.Add(new Hotel
            {
                Name = hotelDTO.Name,
                CurrencyId = hotelDTO.CurrencyId,
                DownPaymentPercentage = hotelDTO.DownPaymentPercentage,
                OwnerId = user.Id
            });

            UserStore.context.SaveChanges();
            return Ok(hotelDTO);
        }

        [HttpPut("{id:int}"), Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public ActionResult<HotelCreateDTO> EditHotel([FromBody] HotelCreateDTO hotelDTO, int id)
        {
            var user = JwtDecoder.GetUser(User.Claims, UserStore.context);
            var hotel = HotelStore.context.Hotels.FirstOrDefault(x => x.Id == id);

            if (hotelDTO == null)
            {
                return BadRequest(hotelDTO);
            }

            if (hotel == null)
            {
                return BadRequest("Hotel does not exist.");
            }

            if (hotel.OwnerId != user.Id)
            {
                return Unauthorized("You are not the owner of this hotel.");
            }

            if (hotelDTO.Name.Length == 0 || hotelDTO.Name.Length > 100)
            {
                return BadRequest("Hotel name must be under 100 characters.");
            }

            hotel.Name = hotelDTO.Name;
            hotel.CurrencyId = hotelDTO.CurrencyId;
            hotel.DownPaymentPercentage = hotelDTO.DownPaymentPercentage;

            HotelStore.context.SaveChanges();
            return Ok(hotelDTO);
        }


        [HttpDelete("{id:int}"), Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteHotel(int id)
        {
            var user = JwtDecoder.GetUser(User.Claims, UserStore.context);
            var hotel = HotelStore.context.Hotels.FirstOrDefault(x => x.Id == id);

            if (hotel == null)
            {
                return BadRequest("Hotel does not exist.");
            }

            if (hotel.OwnerId != user.Id)
            {
                return Unauthorized("You are not the owner of this hotel.");
            }

            HotelStore.context.Hotels.Remove(hotel);
            HotelStore.context.SaveChanges();
            return Ok("Hotel has been deleted.");
        }
    }
}   
