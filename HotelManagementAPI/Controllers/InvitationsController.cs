﻿using HotelManagementAPI.Data;
using HotelManagementAPI.Models.DTO;
using HotelManagementAPI.Models;
using HotelManagementAPI.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;

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
        public IActionResult InviteEmployee([FromBody] HotelCodeCreateDTO HotelCodeDTO)
        {
            User employee = UserStore.context.Users.FirstOrDefault(x => x.Email == HotelCodeDTO.UserEmail);
            Hotel hotel = HotelStore.context.Hotels.FirstOrDefault(x => x.Id == HotelCodeDTO.HotelId);
            var user = JwtDecoder.GetUser(User.Claims, UserStore.context);

            if (employee == null || hotel == null)
            {
                return BadRequest("User or hotel does not exist.");
            }

            var userHotelConnection = HotelStore.context.UsersHotels.FirstOrDefault(x => x.UserId == employee.Id && x.HotelId == hotel.Id);

            if (userHotelConnection != null)
            {
                return BadRequest("User is already an employee of this hotel.");
            }

            if (user.Id != hotel.OwnerId)
            {
                return Unauthorized("You are not the owner of this hotel.");
            }

            if (user == employee)
            {
                return BadRequest("Can't invite yourself to your hotel.");
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
        public IActionResult RespondToInvitation(string codeId, [FromBody] RespondToInviteDTO inviteResponse)
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
        [HttpDelete("{codeId}"), Authorize]
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
