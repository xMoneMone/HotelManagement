using HotelManagementAPI.Models;

namespace HotelManagementAPI.Data
{
    public class AccountTypeStore : DataStore
    {
        public static AccountType? GetById(int id)
        {
            return (from accountType in context.AccountTypes
                    where id == accountType.Id
                    select accountType)
                   .FirstOrDefault();
        }
    }
}
