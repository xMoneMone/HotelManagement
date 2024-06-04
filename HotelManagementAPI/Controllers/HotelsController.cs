using HotelManagementAPI.Data;
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
            var user = JwtDecoder.GetUser(User.Claims);
            return HotelStore.GetUserHotels(user);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpDelete("{hotelId}/employees/{employeeId}"), Authorize(Roles = "Owner")]
        public IActionResult RemoveHotelEmployee(int hotelId, int employeeId)
        {
            var user = JwtDecoder.GetUser(User.Claims);
            var hotel = HotelStore.GetById(hotelId);
            var employee = HotelStore.context.Users.FirstOrDefault(x => x.Id == employeeId);

            var error = HotelValidators.RemoveHotelEmployeeValidator(user, employee, hotel);

            if (error != null)
            {
                return error;
            }

            var userHotelConnection = HotelStore.context.UsersHotels.FirstOrDefault(x => x.UserId == employee.Id && x.HotelId == hotel.Id);

            HotelStore.context.UsersHotels.Remove(userHotelConnection);
            HotelStore.context.SaveChanges();
            return Ok("Employee removed from hotel.");
        }

        [HttpPost, Authorize(Roles = "Owner")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public IActionResult CreateHotel([FromBody] HotelCreateDTO hotelDTO)
        {
            var user = JwtDecoder.GetUser(User.Claims);

            var error = HotelValidators.CreateHotelValidator(hotelDTO);

            if (error != null)
            {
                return error;
            }

            HotelStore.Add(hotelDTO, user);
            
            return Ok("Hotel created successfully.");
        }

        [HttpPut("{id:int}"), Authorize(Roles = "Owner")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult EditHotel([FromBody] HotelCreateDTO hotelDTO, int id)
        {
            var user = JwtDecoder.GetUser(User.Claims);

            var error = HotelValidators.EditHotelValidator(hotelDTO, id, user);

            if (error != null)
            {
                return error;
            }

            HotelStore.Edit(id, hotelDTO);
            
            return Ok("Hotel edited successfully.");
        }


        [HttpDelete("{id:int}"), Authorize(Roles = "Owner")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteHotel(int id)
        {
            var user = JwtDecoder.GetUser(User.Claims);

            var error = HotelValidators.DeleteHotelValidator(id, user);

            if (error != null)
            {
                return error;
            }

            HotelStore.Delete(id);
            return Ok("Hotel has been deleted.");
        }
    }
}   
