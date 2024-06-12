using HotelManagementAPI.Models;
using HotelManagementAPI.Models.DTO;
using HotelManagementAPI.Util;
using Validators = HotelManagementAPI.Util.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelManagementAPI.DataInterfaces;

namespace HotelManagementAPI.Data
{
    public class HotelStore(HotelManagementContext context, IUserStore userStore, ICurrencyStore currencyStore) : IHotelStore
    {
        private readonly HotelManagementContext context = context;
        private readonly IUserStore userStore = userStore;
        private readonly ICurrencyStore currencyStore = currencyStore;

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

            return new ObjectResult("") { StatusCode = 204 };
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

        public async Task<IActionResult> GetHotelCurrencyFormat(int hotelId)
        {
            var hotel = await GetById(hotelId);

            IActionResult? error = HotelValidators.GetHotelCurrencyValidator(hotel);

            if (error != null)
            {
                return error;
            }

            var currency = await currencyStore.GetById(hotel.CurrencyId);
            return new OkObjectResult(currency.FormattingString);
        }

        public async Task<IActionResult> GetDTOById(int id)
        {
            User? user = await userStore.GetCurrentUser();
            Hotel? hotel = await GetById(id);

            IActionResult? error = HotelValidators.GetByIdValidator(hotel, user);

            if (error != null)
            {
                return error;
            }

            Currency? currency = await currencyStore.GetById(hotel.CurrencyId);

            List<EmployeeDTO> hotelEmployees = await (from hotelconnection in context.UsersHotels
                                        join employee in context.Users on hotelconnection.UserId equals employee.Id
                                        where hotelconnection.HotelId == id
                                        select new EmployeeDTO
                                        {
                                            Id = employee.Id,
                                            ColorId = employee.ColorId,
                                            FirstName = employee.FirstName,
                                            LastName = employee.LastName
                                        }).ToListAsync();

            return new OkObjectResult(new HotelDTO
                          {
                              Id = hotel.Id,
                              Name = hotel.Name,
                              CurrencyFormat = currency.FormattingString,
                              DownPaymentPercentage = hotel.DownPaymentPercentage,
                              Employees = hotelEmployees
                          });
        }

        public async Task<IEnumerable<Hotel>> All()
        {
            return await (from hotel in context.Hotels
                          orderby hotel.Name
                          select hotel).ToListAsync();
        }

        public async Task<IActionResult> GetUserHotels()
        {
            var user = await userStore.GetCurrentUser();
            int[] employeeHotelsIds = context.UsersHotels.Where(x => x.UserId == user.Id).Select(x => x.HotelId).ToArray();
            return new OkObjectResult(await (from hotel in context.Hotels
                          where hotel.Owner == user || employeeHotelsIds.Contains(hotel.Id)
                          select new HotelListDTO()
                          {
                              Id = hotel.Id,
                              Name = hotel.Name
                          }).ToListAsync());
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
