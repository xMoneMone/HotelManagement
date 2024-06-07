using HotelManagementAPI.DataInterfaces;
using HotelManagementAPI.Models;
using HotelManagementAPI.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementAPI.Data
{
    public class UserHotelStore(HotelManagementContext context, IUserStore userStore, IHotelStore hotelStore) : IUserHotelStore
    {
        private readonly HotelManagementContext context = context;
        private readonly IUserStore userStore = userStore;
        private readonly IHotelStore hotelStore = hotelStore;

        public async void Add(HotelCode code)
        {
            await context.AddAsync(new UsersHotel
            {
                HotelId = code.HotelId,
                UserId = code.UserId
            });
        }

        public async Task<IActionResult> Delete(int hotelId, int employeeId)
        {
            var user = await userStore.GetCurrentUser();
            var hotel = await hotelStore.GetById(hotelId);
            var employee = await context.Users.FirstOrDefaultAsync(x => x.Id == employeeId);
            var userHotelConnection = await GetByHotelEmployee(hotelId, employeeId);

            var error = HotelValidators.RemoveHotelEmployeeValidator(user, employee, hotel, userHotelConnection);

            if (error != null)
            {
                return error;
            }

            context.UsersHotels.Remove(userHotelConnection);
            await context.SaveChangesAsync();
            return new OkObjectResult("Employee removed from hotel.");
        }

        public async Task<UsersHotel?> GetByHotelEmployee(int? hotelId, int? employeeId)
        {
            if (employeeId == null || hotelId == null)
            {
                return null;
            }
            return await context.UsersHotels.FirstOrDefaultAsync(x => x.UserId == employeeId && x.HotelId == hotelId);
        }
    }
}
