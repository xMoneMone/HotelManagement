using HotelManagementAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementAPI.DataInterfaces
{
    public interface IAccountTypeStore
    {
        AccountType? GetById(int id);
        Task<IActionResult> GetAccountTypes();
    }
}