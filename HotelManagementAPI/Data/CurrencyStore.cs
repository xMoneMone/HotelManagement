using HotelManagementAPI.Models;

namespace HotelManagementAPI.Data
{
    public class CurrencyStore : DataStore
    {
        public static Currency? GetById(int id)
        {
            return (from currency in context.Currencies
                    where id == currency.Id
                    select currency)
                   .FirstOrDefault();
        }
    }
}
