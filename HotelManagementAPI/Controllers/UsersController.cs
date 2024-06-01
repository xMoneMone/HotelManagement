using HotelManagementAPI.Data;
using HotelManagementAPI.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using HotelManagementAPI.Util;
using Microsoft.AspNetCore.JsonPatch;
using HotelManagementAPI.Models;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Validators = HotelManagementAPI.Util.Validators;
using Microsoft.AspNetCore.Authorization;

namespace HotelManagementAPI.Controllers
{
    [Route("users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public UsersController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<UserCreateDTO> CreateUser([FromBody] UserCreateDTO userDTO)
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
                LastName = userDTO.LastName,
                AccountTypeId = userDTO.AccountTypeId
            });

            UserStore.context.SaveChanges();
            return Ok(userDTO);
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<UserLoginDTO> Login(UserLoginDTO userDTO)
        {
            var user = UserStore.context.Users.FirstOrDefault(x => x.Email == userDTO.Email);

            if (user == null)
            {
                return BadRequest("Wrong email or password.");
            }

            if (!BCrypt.Net.BCrypt.Verify(userDTO.Password, user.Password))
            {
                return BadRequest("Wrong email or password.");
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
            var user = JwtDecoder.GetUser(User.Claims, UserStore.context);
            Console.WriteLine(user);

            if (user == null)
            {
                return NotFound();
            }

            UserStore.context.Users.Remove(user);
            UserStore.context.SaveChanges();
            return Ok();
        }

        [HttpPut, Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult EditUser([FromBody] UserEditDTO userDTO)
        {
            userDTO.ColorId = Validators.ColorValidator(userDTO.ColorId);

            var user = JwtDecoder.GetUser(User.Claims, UserStore.context);

            if (user == null)
            {
                return BadRequest();
            }

            user.FirstName = userDTO.FirstName;
            user.LastName = userDTO.LastName;
            user.ColorId = userDTO.ColorId;

            UserStore.context.SaveChanges();

            return Ok();
        }

        [HttpPatch, Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult PatchUser(JsonPatchDocument<User> patch)
        {
            var user = JwtDecoder.GetUser(User.Claims, UserStore.context);

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

        private string CreateToken(User user)
        {
            List<Claim> claims =
            [
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, UserStore.context.AccountTypes.FirstOrDefault(x => x.Id == user.AccountTypeId).Type)
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
