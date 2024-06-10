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
        public async Task<IEnumerable<HotelDTO>> GetHotels()
        {
            return await hotelStore.GetUserHotels();
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpDelete("{hotelId}/employees/{employeeId}"), Authorize(Roles = "Owner")]
        public async Task<IActionResult> RemoveHotelEmployee(int hotelId, int employeeId)
        {
            return await userHotelStore.Delete(hotelId, employeeId);
        }

        [HttpPost, Authorize(Roles = "Owner")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> CreateHotel([FromBody] HotelCreateDTO hotelDTO)
        {

            return await hotelStore.Add(hotelDTO);
            
        }

        [HttpPatch("{id:int}"), Authorize(Roles = "Owner")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> EditHotel([FromBody] HotelCreateDTO hotelDTO, int id)
        {
            return await hotelStore.Edit(id, hotelDTO);
        }


        [HttpDelete("{id:int}"), Authorize(Roles = "Owner")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteHotel(int id)
        {
            return await hotelStore.Delete(id);
        }
    }
}   
