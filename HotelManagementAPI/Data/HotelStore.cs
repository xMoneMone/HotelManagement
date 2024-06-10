using HotelManagementAPI.Models;
using HotelManagementAPI.Models.DTO;
using HotelManagementAPI.Util;
using Validators = HotelManagementAPI.Util.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelManagementAPI.DataInterfaces;

namespace HotelManagementAPI.Data
{
    public class HotelStore(HotelManagementContext context, IUserStore userStore) : IHotelStore
    {
        private readonly HotelManagementContext context = context;
        private readonly IUserStore userStore = userStore;

        public async Task<IActionResult> Add(HotelCreateDTO hotelDTO)
        {
            var user = await userStore.GetCurrentUser();

            var error = HotelValidators.CreateHotelValidator(hotelDTO);

            if (error != null)
            {
                return error;
            }

            await context.Hotels.AddAsync(new Hotel
            {
                Name = hotelDTO.Name,
                CurrencyId = Validators.ValidateMultipleChoice(context.Currencies, hotelDTO.CurrencyId),
                DownPaymentPercentage = hotelDTO.DownPaymentPercentage,
                OwnerId = user.Id
            });
            await context.SaveChangesAsync();

            return new OkObjectResult(hotelDTO);
        }

        public async Task<IActionResult> Edit(int id, HotelCreateDTO hotelDTO)
        {
            var user = await userStore.GetCurrentUser();
            var hotel = await GetById(id);

            var error = HotelValidators.EditHotelValidator(hotelDTO, hotel, user);

            if (error != null)
            {
                return error;
            }

            hotel.Name = hotelDTO.Name;
            hotel.CurrencyId = Validators.ValidateMultipleChoice(context.Currencies, hotelDTO.CurrencyId);
            hotel.DownPaymentPercentage = hotelDTO.DownPaymentPercentage;
            await context.SaveChangesAsync();

            return new OkObjectResult(hotelDTO);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var user = await userStore.GetCurrentUser();
            var hotel = await GetById(id);

            var error = HotelValidators.DeleteHotelValidator(hotel, user);

            if (error != null)
            {
                return error;
            }

            context.Hotels.Remove(hotel);
            await context.SaveChangesAsync();

            return new OkObjectResult("Hotel has been deleted.");
        }

        public async Task<Hotel?> GetById(int? id)
        {
            if (id == null)
            {
                return null;
            }

            return await (from hotel in context.Hotels
                          where id == hotel.Id
                          select hotel)
                   .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Hotel>> All()
        {
            return await (from hotel in context.Hotels
                          select hotel).ToListAsync();
        }

        public async Task<IEnumerable<HotelDTO>> GetUserHotels()
        {
            var user = await userStore.GetCurrentUser();
            int[] employeeHotelsIds = context.UsersHotels.Where(x => x.UserId == user.Id).Select(x => x.HotelId).ToArray();
            return await (from hotel in context.Hotels
                          where hotel.Owner == user || employeeHotelsIds.Contains(hotel.Id)
                          select new HotelDTO()
                          {
                              Id = hotel.Id,
                              Name = hotel.Name,
                              CurrencyFormat = hotel.Currency.FormattingString,
                              DownPaymentPercentage = hotel.DownPaymentPercentage
                          }).ToListAsync();
        }

        public async Task<int[]> GetHotelEmployeesIds(int? hotelId)
        {
            if (hotelId == null)
            {
                return new int[0];
            }
            return await context.UsersHotels.Where(x => x.HotelId == hotelId).Select(x => x.UserId).ToArrayAsync();
        }
    }
}
