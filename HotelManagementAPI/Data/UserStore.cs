using HotelManagementAPI.Models;
using HotelManagementAPI.Models.DTO;
using HotelManagementAPI.Util;

namespace HotelManagementAPI.Data
{
    public class UserStore : DataStore
    {
        public static void Add(UserCreateDTO userDTO)
        {
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
        }

        public static void Edit(User user, UserEditDTO userDTO)
        {
            user.FirstName = userDTO.FirstName;
            user.LastName = userDTO.LastName;
            user.ColorId = Validators.ValidateMultipleChoice(UserStore.context.Colors, userDTO.ColorId);
            context.SaveChanges();
        }

        public static void Delete(User user)
        {
            context.Users.Remove(user);
            context.SaveChanges();
        }

        public static User? GetById(int id)
        {
            return (from user in context.Users
                    where id == user.Id
                    select user)
                   .FirstOrDefault();
        }

        public static User? GetByEmail(string email)
        {
            return (from user in context.Users
                    where email == user.Email
                    select user)
                   .FirstOrDefault();
        }

        public static IEnumerable<User> All()
        {
            return from user in context.Users
                   select user;
        }
    }
}
