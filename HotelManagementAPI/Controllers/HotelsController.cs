using HotelManagementAPI.DataInterfaces;
using HotelManagementAPI.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementAPI.Controllers
{
    [Route(""), Authorize]
    [ApiController]
    public class HotelsController(IHotelStore hotelStore, IUserHotelStore userHotelStore) : ControllerBase
    {
        private readonly IHotelStore hotelStore = hotelStore;
        private readonly IUserHotelStore userHotelStore = userHotelStore;

        [HttpGet("user/hotels"), Authorize]
        public async Task<IEnumerable<HotelDTO>> GetHotels()
        {
            return await hotelStore.GetUserHotels();
        }

        [HttpPost("hotels"), Authorize(Roles = "Owner")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> CreateHotel([FromBody] HotelCreateDTO hotelDTO)
        {

            return await hotelStore.Add(hotelDTO);
            
        }

        [HttpPatch("hotels/{id:int}"), Authorize(Roles = "Owner")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> EditHotel([FromBody] HotelCreateDTO hotelDTO, int id)
        {
            return await hotelStore.Edit(id, hotelDTO);
        }


        [HttpDelete("hotels/{id:int}"), Authorize(Roles = "Owner")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteHotel(int id)
        {
            return await hotelStore.Delete(id);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpDelete("hotels/{hotelId}/employees/{employeeId}"), Authorize(Roles = "Owner")]
        public async Task<IActionResult> RemoveHotelEmployee(int hotelId, int employeeId)
        {
            return await userHotelStore.Delete(hotelId, employeeId);
        }
    }
}   
