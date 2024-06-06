using HotelManagementAPI.DataInterfaces;
using HotelManagementAPI.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementAPI.Controllers
{
    [Route("hotels"), Authorize]
    [ApiController]
    public class HotelsController(IHotelStore hotelStore, IUserHotelStore userHotelStore) : ControllerBase
    {
        private readonly IHotelStore hotelStore = hotelStore;
        private readonly IUserHotelStore userHotelStore = userHotelStore;

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
            return userHotelStore.Delete(hotelId, employeeId);
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
