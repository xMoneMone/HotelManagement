using HotelManagementAPI.Data;
using HotelManagementAPI.DataInterfaces;
using HotelManagementAPI.Models.DTO;
using HotelManagementAPI.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementAPI.Controllers
{
    [Route("hotels"), Authorize]
    [ApiController]
    public class HotelsController(IUserStore userStore, IHotelStore hotelStore) : ControllerBase
    {
        private readonly IUserStore userStore = userStore;
        private readonly IHotelStore hotelStore = hotelStore;

        [HttpGet, Authorize]
        public IEnumerable<HotelDTO> GetHotels()
        {
            return hotelStore.GetUserHotels();
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpDelete("{hotelId}/employees/{employeeId}"), Authorize(Roles = "Owner")]
        public IActionResult RemoveHotelEmployee(int hotelId, int employeeId)
        {
            var user = userStore.GetCurrentUser();
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

            return hotelStore.Add(hotelDTO);
            
        }

        [HttpPut("{id:int}"), Authorize(Roles = "Owner")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult EditHotel([FromBody] HotelCreateDTO hotelDTO, int id)
        {
            return hotelStore.Edit(id, hotelDTO);
        }


        [HttpDelete("{id:int}"), Authorize(Roles = "Owner")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteHotel(int id)
        {
            return hotelStore.Delete(id);
        }
    }
}   
