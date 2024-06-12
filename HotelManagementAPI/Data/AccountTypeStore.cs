using HotelManagementAPI.DataInterfaces;
using HotelManagementAPI.Models;
using HotelManagementAPI.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        public async Task<IActionResult> GetAccountTypes()
        {
            return new OkObjectResult(await (from accountType in context.AccountTypes
                                             orderby accountType.Id
                                             select new AccountTypeDTO
                                             {
                                                 Id = accountType.Id,
                                                 Type = accountType.Type
                                             })
                                             .ToListAsync());
        }
    }
}
