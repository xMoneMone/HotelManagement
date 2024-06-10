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

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("user/invitations"), Authorize]
        public async Task<IActionResult> GetInvites()
        {
            return await hotelCodeStore.GetInvites();
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPost("user/invitations/{codeId}"), Authorize]
        public async Task<IActionResult> RespondToInvitation(string codeId, [FromBody] RespondToInviteDTO inviteResponse)
        {
            return await hotelCodeStore.RespondToInvite(inviteResponse, codeId);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPost("hotels/{hotelId:int}/invitations"), Authorize(Roles = "Owner")]
        public async Task<IActionResult> InviteEmployee([FromBody] HotelCodeCreateDTO hotelCodeDTO, int hotelId)
        {
            return await hotelCodeStore.Add(hotelCodeDTO);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpDelete("hotels/{hotelId:int}/invitations/{codeId}"), Authorize(Roles = "Owner")]
        public async Task<IActionResult> DeleteInvite(string codeId, int hotelId)
        {
            return await hotelCodeStore.Delete(codeId);
        }

    }
}
