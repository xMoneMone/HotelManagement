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

namespace HotelManagementAPI.Controllers
{
    [Route("api/users")]
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
                LastName = userDTO.LastName,
                AccountTypeId = userDTO.AccountTypeId
            });

            UserStore.context.SaveChanges();
            return Ok(userDTO);
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<LoginUserDTO> Login(LoginUserDTO userDTO)
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

        private string CreateToken(User user)
        {
            List<Claim> claims =
            [
                new Claim(ClaimTypes.Email, user.Email),
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
