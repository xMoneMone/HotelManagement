using HotelManagementAPI.Data;
using HotelManagementAPI.Models.DTO;
using HotelManagementAPI.Models;
using HotelManagementAPI.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementAPI.Controllers
{
    [Route("hotels/invitations"), Authorize]
    [ApiController]
    public class InvitationsController : ControllerBase
    {
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPost, Authorize]
        public IActionResult InviteEmployee([FromBody] CreateHotelCodeDTO HotelCodeDTO)
        {
            User employee = UserStore.context.Users.FirstOrDefault(x => x.Email == HotelCodeDTO.UserEmail);
            Hotel hotel = HotelStore.context.Hotels.FirstOrDefault(x => x.Id == HotelCodeDTO.HotelId);
            var user = JwtDecoder.GetUser(User.Claims, UserStore.context);

            if (employee == null || hotel == null)
            {
                return BadRequest("User or hotel does not exist.");
            }

            if (user.Id != hotel.OwnerId)
            {
                return Unauthorized("You are not the owner of this hotel.");
            }

            string code = CodeGenerator.GenerateCode();
            var hotelCode = new HotelCode
            {
                UserId = user.Id,
                Code = code,
                HotelId = HotelCodeDTO.HotelId
            };

            HotelStore.context.HotelCodes.Add(hotelCode);
            HotelStore.context.SaveChanges();
            return Ok(code);
        }
    }
}
