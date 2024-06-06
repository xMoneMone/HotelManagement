using HotelManagementAPI.DataInterfaces;
using HotelManagementAPI.Models;
using HotelManagementAPI.Models.DTO;
using HotelManagementAPI.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Validators = HotelManagementAPI.Util.Validators;

namespace HotelManagementAPI.Data
{
    public class UserStore(HotelManagementContext _context, IHttpContextAccessor http, IConfiguration configuration, IAccountTypeStore accountTypeStore) : IUserStore
    {
        private readonly HotelManagementContext context = _context;
        private readonly IHttpContextAccessor http = http;
        private readonly IConfiguration configuration = configuration;
        private readonly IAccountTypeStore accountTypeStore = accountTypeStore;

        public User? GetCurrentUser()
        {
            var userId = Jwt.DecodeUser(http.HttpContext.User.Claims);
            return GetById(userId);
        }

        public IActionResult Add(UserCreateDTO userDTO)
        {
            var error = UserValidators.CreateUserValidator(userDTO);

            if (error != null)
            {
                return error;
            }

            context.Users.Add(new Models.User
            {
                ColorId = Validators.ValidateMultipleChoice(context.Colors, userDTO.ColorId),
                Email = userDTO.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(userDTO.Password),
                FirstName = userDTO.FirstName,
                LastName = userDTO.LastName,
                AccountTypeId = Validators.ValidateMultipleChoice(context.AccountTypes, userDTO.ColorId)
            });
            context.SaveChanges();

            return new OkObjectResult("User has been created.");
        }

        public IActionResult Edit(UserEditDTO userDTO)
        {
            var user = GetCurrentUser();

            var error = UserValidators.EditUserValidator(userDTO);

            if (error != null)
            {
                return error;
            }

            user.FirstName = userDTO.FirstName;
            user.LastName = userDTO.LastName;
            user.ColorId = Validators.ValidateMultipleChoice(context.Colors, userDTO.ColorId);
            context.SaveChanges();

            return new OkObjectResult("User has been edited.");
        }

        public IActionResult Delete()
        {
            var user = GetCurrentUser();
            context.Users.Remove(user);
            context.SaveChanges();
            return new OkObjectResult("User has been deleted.");
        }

        public User? GetById(int id)
        {
            return (from user in context.Users
                    where id == user.Id
                    select user)
                   .FirstOrDefault();
        }

        public User? GetByEmail(string email)
        {
            return (from user in context.Users
                    where email == user.Email
                    select user)
                   .FirstOrDefault();
        }

        public IActionResult Login(UserLoginDTO userDTO)
        {
            var user = GetByEmail(userDTO.Email);

            var error = UserValidators.LoginValidator(userDTO, user);

            if (error != null)
            {
                return error;
            }

            return new OkObjectResult(CreateToken(user));
        }

        public IEnumerable<User> All()
        {
            return from user in context.Users
                   select user;
        }

        private string CreateToken(User user)
        {
            List<Claim> claims =
            [
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, accountTypeStore.GetById(user.AccountTypeId).Type)
            ];

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("AppSettings:Token").Value!));
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
