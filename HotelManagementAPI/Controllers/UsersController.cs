using HotelManagementAPI.Data;
using HotelManagementAPI.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementAPI.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<CreateUserDTO> CreateUser([FromBody]CreateUserDTO userDTO)
        {
            if (userDTO == null)
            {
                return BadRequest(userDTO);
            }

            int[] colors = UserStore.context.Colors.Select(x => x.Id).ToArray();
            if (!colors.Contains(userDTO.ColorId))
            {
                userDTO.ColorId = colors[0];
            }

            UserStore.context.Users.Add(new Models.User
            {
                ColorId = userDTO.ColorId,
                Email = userDTO.Email,
                Password = userDTO.Password,
                FirstName = userDTO.FirstName,
                LastName = userDTO.LastName
            });

            UserStore.context.SaveChanges();
            return Ok(userDTO);
        }
    }
}
