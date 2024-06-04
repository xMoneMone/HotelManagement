using HotelManagementAPI.Models;
using HotelManagementAPI.Models.DTO;
using HotelManagementAPI.Util;
using Microsoft.IdentityModel.Tokens;
using Validators = HotelManagementAPI.Util.Validators;
using System.Runtime.CompilerServices;

namespace HotelManagementAPI.Data
{
    public class HotelStore : DataStore
    {
        public static void Add(HotelCreateDTO hotelDTO, User user)
        {
            context.Hotels.Add(new Hotel
            {
                Name = hotelDTO.Name,
                CurrencyId = Validators.ValidateMultipleChoice(context.Currencies, hotelDTO.CurrencyId),
                DownPaymentPercentage = hotelDTO.DownPaymentPercentage,
                OwnerId = user.Id
            });
            context.SaveChanges();
        }

        public static void Edit(int id, HotelCreateDTO hotelDTO)
        {
            var hotel = GetById(id);
            hotel.Name = hotelDTO.Name;
            hotel.CurrencyId = Validators.ValidateMultipleChoice(context.Currencies, hotelDTO.CurrencyId);
            hotel.DownPaymentPercentage = hotelDTO.DownPaymentPercentage;
            context.SaveChanges();
        }

        public static void Delete(int id)
        {
            var hotel = GetById(id);
            context.Hotels.Remove(hotel);
            context.SaveChanges();
        }

        public static Hotel? GetById(int id)
        {
            return (from hotel in context.Hotels
                    where id == hotel.Id
                    select hotel)
                   .FirstOrDefault();
        }

        public static IEnumerable<Hotel> All()
        {
            return from hotel in context.Hotels
                   select hotel;
        }

        public static IEnumerable<HotelDTO> GetUserHotels(User user)
        {
            int[] employeeHotelsIds = UserStore.context.UsersHotels.Where(x => x.UserId == user.Id).Select(x => x.HotelId).ToArray();
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

        public static int[] GetHotelEmployeesIds(int hotelId)
        {
            return UserStore.context.UsersHotels.Where(x => x.HotelId == hotelId).Select(x => x.UserId).ToArray();
        }
    }
}
