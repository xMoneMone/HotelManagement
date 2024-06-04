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
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet, Authorize]
        public IActionResult GetInvites()
        {
            var user = JwtDecoder.GetUser(User.Claims, UserStore.context);

            if (user.AccountTypeId == 1)
            {
                return Ok(HotelCodeStore.GetSentInvites(user));
            }
            else if (user.AccountTypeId == 2)
            {
                return Ok(HotelCodeStore.GetReceivedInvites(user));
            }

            return StatusCode(500);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPost, Authorize(Roles = "Owner")]
        public IActionResult InviteEmployee([FromBody] HotelCodeCreateDTO HotelCodeDTO)
        {
            User employee = UserStore.context.Users.FirstOrDefault(x => x.Email == HotelCodeDTO.UserEmail);
            Hotel hotel = HotelStore.context.Hotels.FirstOrDefault(x => x.Id == HotelCodeDTO.HotelId);
            var user = JwtDecoder.GetUser(User.Claims, UserStore.context);

            var error = InvitationValidators.InviteEmployeeValidator(user, employee, hotel);

            if (error != null)
            {
                return error;
            }

            var userHotelConnection = HotelStore.context.UsersHotels.FirstOrDefault(x => x.UserId == employee.Id && x.HotelId == hotel.Id);

            string code = CodeGenerator.GenerateCode();
            var hotelCode = new HotelCode
            {
                UserId = employee.Id,
                Code = code,
                HotelId = HotelCodeDTO.HotelId,
                StatusId = 1,
                SenderId = user.Id
            };

            HotelStore.context.HotelCodes.Add(hotelCode);
            HotelStore.context.SaveChanges();
            return Ok(code);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPost("{codeId}"), Authorize]
        public IActionResult RespondToInvitation(string codeId, [FromBody] RespondToInviteDTO inviteResponse)
        {
            var code = HotelStore.context.HotelCodes.FirstOrDefault(x => x.Code == codeId);
            var user = JwtDecoder.GetUser(User.Claims, UserStore.context);

            var error = InvitationValidators.RespondToInvitationValidator(user, code);

            if (error != null)
            {
                return error;
            }

            if (inviteResponse.Accept)
            {
                code.StatusId = 2;
                var hotelEmployeeConnection = new UsersHotel
                {
                    HotelId = code.HotelId,
                    UserId = code.UserId
                };
                HotelStore.context.UsersHotels.Add(hotelEmployeeConnection);
                HotelStore.context.SaveChanges();
                return Ok("Invite accepted.");
            }
            else
            {
                code.StatusId = 3;
                HotelStore.context.SaveChanges();
                return Ok("Invite rejected.");
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpDelete("{codeId}"), Authorize(Roles = "Owner")]
        public IActionResult DeleteInvite(string codeId)
        {
            var code = HotelStore.context.HotelCodes.FirstOrDefault(x => x.Code == codeId);

            if (code == null)
            {
                return BadRequest("Code does not exist.");
            }

            Hotel hotel = HotelStore.context.Hotels.FirstOrDefault(x => x.Id == code.HotelId);
            var hotelOwner = UserStore.context.Users.FirstOrDefault(x => x.Id == hotel.OwnerId);
            var user = JwtDecoder.GetUser(User.Claims, UserStore.context);

            if (user.Id != hotelOwner.Id)
            {
                return Unauthorized("You're not the creator of this invite.");
            }

            HotelStore.context.HotelCodes.Remove(code);
            HotelStore.context.SaveChanges();

            return Ok("Invite deleted.");
        }

    }
}
