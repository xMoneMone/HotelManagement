using HotelManagementAPI.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HotelManagementAPI.DataInterfaces;

namespace HotelManagementAPI.Controllers
{
    [Route(""), Authorize]
    [ApiController]
    public class InvitationsController(IHotelCodeStore hotelCodeStore) : ControllerBase
    {
        private readonly IHotelCodeStore hotelCodeStore = hotelCodeStore;

        [HttpGet("user/invitations"), Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetInvites()
        {
            return await hotelCodeStore.GetInvites();
        }

        [HttpPost("user/invitations/{codeId}"), Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RespondToInvitation(string codeId, [FromBody] RespondToInviteDTO inviteResponse)
        {
            return await hotelCodeStore.RespondToInvite(inviteResponse, codeId);
        }

        [HttpPost("hotels/{hotelId}/invitations"), Authorize(Roles = "Owner")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> InviteEmployee([FromBody] HotelCodeCreateDTO hotelCodeDTO)
        {
            return await hotelCodeStore.Add(hotelCodeDTO);
        }

        [HttpDelete("hotels/{hotelId}/invitations/{codeId}"), Authorize(Roles = "Owner")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteInvite(string codeId)
        {
            return await hotelCodeStore.Delete(codeId);
        }

    }
}
