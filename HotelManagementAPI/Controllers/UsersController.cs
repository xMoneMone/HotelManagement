using HotelManagementAPI.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using HotelManagementAPI.DataInterfaces;

namespace HotelManagementAPI.Controllers
{
    [Route("users")]
    [ApiController]
    public class UsersController(IUserStore userStore) : ControllerBase
    {
        private readonly IUserStore userStore = userStore;

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CreateUser([FromBody] UserCreateDTO userDTO)
        {
            return userStore.Add(userDTO);
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Login(UserLoginDTO userDTO)
        {
            return userStore.Login(userDTO);
        }

        [HttpDelete, Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteUser()
        {
            return userStore.Delete();
        }

        [HttpPut, Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult EditUser([FromBody] UserEditDTO userDTO)
        {
            return userStore.Edit(userDTO);
        } 
    }
}
