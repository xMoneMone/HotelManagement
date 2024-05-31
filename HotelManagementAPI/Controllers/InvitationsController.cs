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
                UserId = employee.Id,
                Code = code,
                HotelId = HotelCodeDTO.HotelId,
                StatusId = 1
            };

            HotelStore.context.HotelCodes.Add(hotelCode);
            HotelStore.context.SaveChanges();
            return Ok(code);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPost("{codeId}"), Authorize]
        public IActionResult AcceptInvitation(string codeId)
        {
            var code = HotelStore.context.HotelCodes.FirstOrDefault(x => x.Code == codeId);
            var user = JwtDecoder.GetUser(User.Claims, UserStore.context);

            if (code == null)
            {
                return BadRequest("Code does not exist.");
            }

            if (user.Id != code.UserId)
            {
                return Unauthorized("You are not the recepient of this invite.");
            }

            if (code.StatusId != 1)
            {
                return BadRequest("Code already used.");
            }

            code.StatusId = 2;
            var hotelEmployeeConnection = new UsersHotel
            {
                HotelId = code.HotelId,
                UserId = code.UserId
            };

            HotelStore.context.UsersHotels.Add(hotelEmployeeConnection);
            HotelStore.context.SaveChanges();

            return Ok("Employee added to hotel.");
        }
    }
}
