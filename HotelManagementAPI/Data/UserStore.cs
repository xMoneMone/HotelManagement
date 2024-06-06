using HotelManagementAPI.DataInterfaces;
using HotelManagementAPI.Models;
using HotelManagementAPI.Models.DTO;
using HotelManagementAPI.Util;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace HotelManagementAPI.Data
{
    public class UserStore(HotelManagementContext _context, IHttpContextAccessor http) : IUserStore
    {
        private readonly HotelManagementContext context = _context;
        private readonly IHttpContextAccessor http = http;

        public User? GetCurrentUser()
        {
            var userId = JwtDecoder.GetUser(http.HttpContext.User.Claims);
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

        public IEnumerable<User> All()
        {
            return from user in context.Users
                   select user;
        }
    }
}
