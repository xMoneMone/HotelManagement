using HotelManagementAPI.DataInterfaces;
using HotelManagementAPI.Models;

namespace HotelManagementAPI.Data
{
    public class AccountTypeStore(HotelManagementContext context) : IAccountTypeStore
    {
        private readonly HotelManagementContext context = context;

        public AccountType? GetById(int id)
        {
            return (from accountType in context.AccountTypes
                    where id == accountType.Id
                    select accountType)
                   .FirstOrDefault();
        }
    }
}
