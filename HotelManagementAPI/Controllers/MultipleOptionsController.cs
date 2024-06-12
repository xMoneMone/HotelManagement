using HotelManagementAPI.DataInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementAPI.Controllers
{
    [Route("multiple-options")]
    [ApiController]
    public class MultipleOptionsController(IColorStore colorStore, IAccountTypeStore accountTypeStore) : ControllerBase
    {
        private readonly IColorStore colorStore = colorStore;
        private readonly IAccountTypeStore accountTypeStore = accountTypeStore;

        [HttpGet("colors")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetHotels()
        {
            return await colorStore.GetColors();
        }

        [HttpGet("account-types")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAccountTypes()
        {
            return await accountTypeStore.GetAccountTypes();
        }
    }
}
