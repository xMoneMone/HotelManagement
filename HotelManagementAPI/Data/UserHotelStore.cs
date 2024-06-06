using HotelManagementAPI.DataInterfaces;
using HotelManagementAPI.Models;
using HotelManagementAPI.Util;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementAPI.Data
{
    public class UserHotelStore(HotelManagementContext context, IUserStore userStore, IHotelStore hotelStore) : IUserHotelStore
    {
        private readonly HotelManagementContext context = context;
        private readonly IUserStore userStore = userStore;
        private readonly IHotelStore hotelStore = hotelStore;

        public void Add(HotelCode code)
        {
            context.Add(new UsersHotel
            {
                HotelId = code.HotelId,
                UserId = code.UserId
            });
        }

        public IActionResult Delete(int hotelId, int employeeId)
        {
            var user = userStore.GetCurrentUser();
            var hotel = hotelStore.GetById(hotelId);
            var employee = context.Users.FirstOrDefault(x => x.Id == employeeId);
            var userHotelConnection = GetByHotelEmployee(hotelId, employeeId);

            var error = HotelValidators.RemoveHotelEmployeeValidator(user, employee, hotel, userHotelConnection);

            if (error != null)
            {
                return error;
            }

            context.UsersHotels.Remove(userHotelConnection);
            context.SaveChanges();
            return new OkObjectResult("Employee removed from hotel.");
        }

        public UsersHotel? GetByHotelEmployee(int? hotelId, int? employeeId)
        {
            if (employeeId == null || hotelId == null)
            {
                return null;
            }
            return context.UsersHotels.FirstOrDefault(x => x.UserId == employeeId && x.HotelId == hotelId);
        }
    }
}
