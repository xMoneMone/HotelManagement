using HotelManagementAPI.Data;
using HotelManagementAPI.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using HotelManagementAPI.Util;
using HotelManagementAPI.Models;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using HotelManagementAPI.DataInterfaces;

namespace HotelManagementAPI.Controllers
{
    [Route("users")]
    [ApiController]
    public class UsersController(IConfiguration configuration, IUserStore userStore) : ControllerBase
    {
        private readonly IConfiguration _configuration = configuration;
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
            var user = userStore.GetByEmail(userDTO.Email);

            var error = UserValidators.LoginValidator(userDTO, user);

            if (error != null)
            {
                return error;
            }

            string token = CreateToken(user);

            return Ok(token);
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

        private string CreateToken(User user)
        {
            List<Claim> claims =
            [
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, AccountTypeStore.GetById(user.AccountTypeId).Type)
            ];

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(30),
                    signingCredentials: credentials
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        } 
    }
}
