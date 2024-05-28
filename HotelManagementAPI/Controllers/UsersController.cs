using HotelManagementAPI.Data;
using HotelManagementAPI.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using HotelManagementAPI.Util;
using Microsoft.AspNetCore.JsonPatch;
using HotelManagementAPI.Models;

namespace HotelManagementAPI.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<CreateUserDTO> CreateUser([FromBody] CreateUserDTO userDTO)
        {
            if (userDTO == null)
            {
                return BadRequest(userDTO);
            }

            userDTO.ColorId = Validators.ColorValidator(userDTO.ColorId);

            UserStore.context.Users.Add(new Models.User
            {
                ColorId = userDTO.ColorId,
                Email = userDTO.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(userDTO.Password),
                FirstName = userDTO.FirstName,
                LastName = userDTO.LastName
            });

            UserStore.context.SaveChanges();
            return Ok(userDTO);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteUser(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            var user = UserStore.context.Users.FirstOrDefault(x => x.Id == id);
            Console.WriteLine(user);

            if (user == null)
            {
                return NotFound();
            }

            UserStore.context.Users.Remove(user);
            UserStore.context.SaveChanges();
            return Ok();
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult EditUser(int id, [FromBody] EditUserDTO userDTO)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            userDTO.ColorId = Validators.ColorValidator(userDTO.ColorId);

            var user = UserStore.context.Users.FirstOrDefault(x => x.Id == id);
            user.FirstName = userDTO.FirstName;
            user.LastName = userDTO.LastName;
            user.ColorId = userDTO.ColorId;

            UserStore.context.SaveChanges();

            return Ok();
        }

        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult PatchUser(int id, JsonPatchDocument<User> patch)
        {
            if (id == 0 || patch == null)
            {
                return BadRequest();
            }

            var user = UserStore.context.Users.FirstOrDefault(x => x.Id == id);

            if (user == null)
            {
                return BadRequest();
            }

            patch.ApplyTo(user, ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            UserStore.context.SaveChanges();
            return Ok();
        }
    }
}
