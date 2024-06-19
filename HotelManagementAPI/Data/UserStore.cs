using HotelManagementAPI.DataInterfaces;
using HotelManagementAPI.Models;
using HotelManagementAPI.Models.DTO;
using HotelManagementAPI.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Validators = HotelManagementAPI.Util.Validators;

namespace HotelManagementAPI.Data
{
    public class UserStore(HotelManagementContext _context, IHttpContextAccessor http, IConfiguration configuration, IAccountTypeStore accountTypeStore, IColorStore colorStore) : IUserStore
    {
        private readonly HotelManagementContext context = _context;
        private readonly IHttpContextAccessor http = http;
        private readonly IConfiguration configuration = configuration;
        private readonly IAccountTypeStore accountTypeStore = accountTypeStore;
        private readonly IColorStore colorStore = colorStore;

        public async Task<User?> GetCurrentUser()
        {
            // IS THIS SECURE?????
            var userId = Jwt.DecodeUser(http.HttpContext.User.Claims);
            return await GetById(userId);
        }

        public async Task<IActionResult> Add(UserCreateDTO userDTO)
        {
            var error = UserValidators.CreateUserValidator(userDTO);

            if (error != null)
            {
                return error;
            }

            var newUser = new Models.User
            {
                ColorId = Validators.ValidateMultipleChoice(context.Colors, userDTO.ColorId),
                Email = userDTO.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(userDTO.Password),
                FirstName = userDTO.FirstName,
                LastName = userDTO.LastName,
                AccountTypeId = Validators.ValidateMultipleChoice(context.AccountTypes, userDTO.ColorId)
            };

            await context.Users.AddAsync(newUser);
            await context.SaveChangesAsync();

            return new OkObjectResult(userDTO);
        }

        public async Task<IActionResult> Edit(UserEditDTO userDTO)
        {
            var user = await GetCurrentUser();

            var error = UserValidators.EditUserValidator(userDTO);

            if (error != null)
            {
                return error;
            }

            userDTO.ColorId = Validators.ValidateMultipleChoice(context.Colors, userDTO.ColorId);

            user.FirstName = userDTO.FirstName;
            user.LastName = userDTO.LastName;
            user.ColorId = userDTO.ColorId;
            await context.SaveChangesAsync();

            return new OkObjectResult(userDTO);
        }

        public async Task<IActionResult> Delete()
        {
            var user = await GetCurrentUser();
            context.Users.Remove(user);
            await context.SaveChangesAsync();
            return new ObjectResult("") { StatusCode = 204 };
        }

        public async Task<User?> GetById(int id)
        {
            return await (from user in context.Users
                          where id == user.Id
                          select user)
                   .FirstOrDefaultAsync();
        }

        public async Task<IActionResult> GetUserDTO()
        {
            var user = await GetCurrentUser();

            return new OkObjectResult(new UserDTO
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Color = await colorStore.GetColorById(user.ColorId),
                AccountType = user.AccountTypeId
            });
        }

        public async Task<User?> GetByEmail(string email)   
        {
            return await (from user in context.Users
                          where email == user.Email
                          select user)
                   .FirstOrDefaultAsync();
        }

        public async Task<IActionResult> Login(UserLoginDTO userDTO)
        {
            var user = await GetByEmail(userDTO.Email);

            var error = UserValidators.LoginValidator(userDTO, user);

            if (error != null)
            {
                return error;
            }

            return new OkObjectResult(CreateToken(user));
        }

        public async Task<IEnumerable<User>> All()
        {
            return await (from user in context.Users
                          select user).ToListAsync();
        }

        private string CreateToken(User? user)
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
