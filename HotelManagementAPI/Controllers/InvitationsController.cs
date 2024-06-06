using HotelManagementAPI.Models.DTO;
using HotelManagementAPI.Models;
using HotelManagementAPI.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HotelManagementAPI.DataInterfaces;

namespace HotelManagementAPI.Controllers
{
    [Route("hotels/invitations"), Authorize]
    [ApiController]
    public class InvitationsController(IHotelCodeStore hotelCodeStore) : ControllerBase
    {
        private readonly IHotelCodeStore hotelCodeStore = hotelCodeStore;

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet, Authorize]
        public IActionResult GetInvites()
        {
            return hotelCodeStore.GetInvites();
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPost, Authorize(Roles = "Owner")]
        public IActionResult InviteEmployee([FromBody] HotelCodeCreateDTO hotelCodeDTO)
        {
            return hotelCodeStore.Add(hotelCodeDTO);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPost("{codeId}"), Authorize]
        public IActionResult RespondToInvitation(string codeId, [FromBody] RespondToInviteDTO inviteResponse)
        {
            return hotelCodeStore.RespondToInvite(inviteResponse, codeId);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpDelete("{codeId}"), Authorize(Roles = "Owner")]
        public IActionResult DeleteInvite(string codeId)
        {
            return hotelCodeStore.Delete(codeId);
        }

    }
}
