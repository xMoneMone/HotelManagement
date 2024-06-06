using HotelManagementAPI.Models;
using HotelManagementAPI.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementAPI.DataInterfaces
{
    public interface IUserStore
    {
        IActionResult Add(UserCreateDTO userDTO);
        IEnumerable<User> All();
        IActionResult Delete();
        IActionResult Edit(UserEditDTO userDTO);
        User? GetByEmail(string email);
        User? GetById(int id);
        User? GetCurrentUser();
    }
}