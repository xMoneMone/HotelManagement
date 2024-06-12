using HotelManagementAPI.DataInterfaces;
using HotelManagementAPI.Models;
using HotelManagementAPI.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementAPI.Data
{
    public class CurrencyStore(HotelManagementContext context) : ICurrencyStore
    {
        private readonly HotelManagementContext context = context;

        public async Task<Currency?> GetById(int? id)
        {
            if (id == null)
            {
                return null;
            }
            return await (from currency in context.Currencies
                    where id == currency.Id
                    select currency)
                   .FirstOrDefaultAsync();
        }

        public async Task<IActionResult> GetCurrencies()
        {
            return new OkObjectResult(await (from currency in context.Currencies
                                             orderby currency.Id
                                             select new CurrencyDTO
                                             {
                                                 Id = currency.Id,
                                                 Name = currency.Name,
                                             }).ToListAsync());
        }
    }
}
