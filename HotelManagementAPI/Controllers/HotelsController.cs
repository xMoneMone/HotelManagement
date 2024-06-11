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
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetHotels()
        {
            return await hotelStore.GetUserHotels();
        }

        [HttpGet("hotels/{hotelId:int}"), Authorize(Roles = "Owner")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int hotelId)
        {
            return await hotelStore.GetDTOById(hotelId);
        }

        [HttpPost("hotels"), Authorize(Roles = "Owner")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateHotel([FromBody] HotelCreateDTO hotelDTO)
        {

            return await hotelStore.Add(hotelDTO);
            
        }

        [HttpPatch("hotels/{id:int}"), Authorize(Roles = "Owner")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> EditHotel([FromBody] HotelCreateDTO hotelDTO, int id)
        {
            return await hotelStore.Edit(id, hotelDTO);
        }


        [HttpDelete("hotels/{id:int}"), Authorize(Roles = "Owner")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteHotel(int id)
        {
            return await hotelStore.Delete(id);
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("hotels/{hotelId}/employees/{employeeId}"), Authorize(Roles = "Owner")]
        public async Task<IActionResult> RemoveHotelEmployee(int hotelId, int employeeId)
        {
            return await userHotelStore.Delete(hotelId, employeeId);
        }
    }
}   
