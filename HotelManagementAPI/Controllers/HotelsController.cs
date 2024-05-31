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
            var user = JwtDecoder.GetUser(User.Claims, UserStore.context);
            return HotelStore.GetUserHotels(user);
        }
    }
}   
