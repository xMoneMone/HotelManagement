using HotelManagementAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementAPI.DataInterfaces
{
    public interface ICurrencyStore
    {
        Task<Currency?> GetById(int? id);
        Task<IActionResult> GetCurrencies();
    }
}