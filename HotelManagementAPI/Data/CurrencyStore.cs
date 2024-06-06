using HotelManagementAPI.DataInterfaces;
using HotelManagementAPI.Models;

namespace HotelManagementAPI.Data
{
    public class CurrencyStore(HotelManagementContext context) : ICurrencyStore
    {
        private readonly HotelManagementContext context = context;

        public Currency? GetById(int id)
        {
            return (from currency in context.Currencies
                    where id == currency.Id
                    select currency)
                   .FirstOrDefault();
        }
    }
}
