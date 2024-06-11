using HotelManagementAPI.DataInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementAPI.Controllers
{
    [Route("colors")]
    [ApiController]
    public class ColorsController(IColorStore colorStore) : ControllerBase
    {
        private readonly IColorStore colorStore = colorStore;

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetHotels()
        {
            return await colorStore.GetColors();
        }
    }
}
