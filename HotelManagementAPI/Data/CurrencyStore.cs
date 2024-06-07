using HotelManagementAPI.DataInterfaces;
using HotelManagementAPI.Models;
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
    }
}
