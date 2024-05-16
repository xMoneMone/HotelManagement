using HotelManagementAPI.Data;
using HotelManagementAPI.Models;
using HotelManagementAPI.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementAPI.Controllers
{
    [Route("api/hotels")]
    [ApiController]
    public class HotelsController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<HotelDTO> GetHotels()
        {
            return HotelStore.allHotels;
        }
    }
}   
