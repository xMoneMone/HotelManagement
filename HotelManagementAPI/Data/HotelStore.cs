using HotelManagementAPI.Models;
using HotelManagementAPI.Models.DTO;
using HotelManagementAPI.Util;
using Microsoft.IdentityModel.Tokens;
using Validators = HotelManagementAPI.Util.Validators;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using HotelManagementAPI.DataInterfaces;

namespace HotelManagementAPI.Data
{
    public class HotelStore(HotelManagementContext context, IUserStore userStore) : IHotelStore
    {
        private readonly HotelManagementContext context = context;
        private readonly IUserStore userStore = userStore;

        public IActionResult Add(HotelCreateDTO hotelDTO)
        {
            var user = userStore.GetCurrentUser();

            var error = HotelValidators.CreateHotelValidator(hotelDTO);

            if (error != null)
            {
                return error;
            }

            context.Hotels.Add(new Hotel
            {
                Name = hotelDTO.Name,
                CurrencyId = Validators.ValidateMultipleChoice(context.Currencies, hotelDTO.CurrencyId),
                DownPaymentPercentage = hotelDTO.DownPaymentPercentage,
                OwnerId = user.Id
            });
            context.SaveChanges();

            return new OkObjectResult("Hotel created successfully.");
        }

        public IActionResult Edit(int id, HotelCreateDTO hotelDTO)
        {
            var user = userStore.GetCurrentUser();
            var hotel = GetById(id);

            var error = HotelValidators.EditHotelValidator(hotelDTO, hotel, user);

            if (error != null)
            {
                return error;
            }

            hotel.Name = hotelDTO.Name;
            hotel.CurrencyId = Validators.ValidateMultipleChoice(context.Currencies, hotelDTO.CurrencyId);
            hotel.DownPaymentPercentage = hotelDTO.DownPaymentPercentage;
            context.SaveChanges();

            return new OkObjectResult("Hotel edited successfully.");
        }

        public IActionResult Delete(int id)
        {
            var user = userStore.GetCurrentUser();
            var hotel = GetById(id);

            var error = HotelValidators.DeleteHotelValidator(hotel, user);

            if (error != null)
            {
                return error;
            }

            context.Hotels.Remove(hotel);
            context.SaveChanges();

            return new OkObjectResult("Hotel has been deleted.");
        }

        public Hotel? GetById(int? id)
        {
            if (id == null)
            {
                return null;
            }

            return (from hotel in context.Hotels
                    where id == hotel.Id
                    select hotel)
                   .FirstOrDefault();
        }

        public IEnumerable<Hotel> All()
        {
            return from hotel in context.Hotels
                   select hotel;
        }

        public IEnumerable<HotelDTO> GetUserHotels()
        {
            var user = userStore.GetCurrentUser();
            int[] employeeHotelsIds = context.UsersHotels.Where(x => x.UserId == user.Id).Select(x => x.HotelId).ToArray();
            return from hotel in context.Hotels
                   where hotel.Owner == user || employeeHotelsIds.Contains(hotel.Id)
                   select new HotelDTO()
                   {
                       Id = hotel.Id,
                       Name = hotel.Name,
                       CurrencyFormat = hotel.Currency.FormattingString,
                       DownPaymentPercentage = hotel.DownPaymentPercentage
                   };
        }

        public int[] GetHotelEmployeesIds(int? hotelId)
        {
            if (hotelId == null)
            {
                return new int[0];
            }
            return [.. context.UsersHotels.Where(x => x.HotelId == hotelId).Select(x => x.UserId)];
        }
    }
}
