using HotelManagementAPI.Models;
using HotelManagementAPI.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementAPI.DataInterfaces
{
    public interface IUserStore
    {
        Task<IActionResult> Add(UserCreateDTO userDTO);
        Task<IEnumerable<User>> All();
        Task<IActionResult> Delete();
        Task<IActionResult> Edit(UserEditDTO userDTO);
        Task<User?> GetByEmail(string email);
        Task<User?> GetById(int id);
        Task<User?> GetCurrentUser();
        Task<IActionResult> Login(UserLoginDTO userDTO);
    }
}