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
            var user = JwtDecoder.GetUser(User.Claims, DataStore.context);

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
        public IActionResult InviteEmployee([FromBody] HotelCodeCreateDTO hotelCodeDTO)
        {
            var user = JwtDecoder.GetUser(User.Claims, DataStore.context);

            var error = InvitationValidators.InviteEmployeeValidator(user, hotelCodeDTO.UserEmail, hotelCodeDTO.HotelId);

            if (error != null)
            {
                return error;
            }

            string code = CodeGenerator.GenerateCode();
            HotelCodeStore.Add(hotelCodeDTO, code, user);

            return Ok(code);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPost("{codeId}"), Authorize]
        public IActionResult RespondToInvitation(string codeId, [FromBody] RespondToInviteDTO inviteResponse)
        {
            var user = JwtDecoder.GetUser(User.Claims, DataStore.context);

            var error = InvitationValidators.RespondToInvitationValidator(user, codeId);

            if (error != null)
            {
                return error;
            }

            if (inviteResponse.Accept)
            {
                HotelCodeStore.AcceptInvite(codeId);
                return Ok("Invite accepted.");
            }
            else
            {
                HotelCodeStore.RejectInvite(codeId);
                return Ok("Invite rejected.");
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpDelete("{codeId}"), Authorize(Roles = "Owner")]
        public IActionResult DeleteInvite(string codeId)
        {
            var user = JwtDecoder.GetUser(User.Claims, DataStore.context);

            var error = InvitationValidators.DeleteInviteValidator(user, codeId);

            if (error != null)
            {
                return error;
            }

            HotelCodeStore.Delete(codeId);
            return Ok("Invite deleted.");
        }

    }
}
