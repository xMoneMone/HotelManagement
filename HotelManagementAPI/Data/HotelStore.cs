using HotelManagementAPI.Models;
using HotelManagementAPI.Models.DTO;
using System.Runtime.CompilerServices;

namespace HotelManagementAPI.Data
{
    public class HotelStore
    {
        public static HotelManagementContext context = new HotelManagementContext();
        public static IEnumerable<HotelDTO> allHotels = from hotel in context.Hotels
                                                        select new HotelDTO()
                                                        {
                                                            Id = hotel.Id,
                                                            Name = hotel.Name,
                                                            CurrencyFormat = hotel.Currency.FormattingString,
                                                            DownPaymentPercentage = hotel.DownPaymentPercentage
                                                        };

        public static IEnumerable<HotelDTO> GetUserHotels(User user)
        {
            return from hotel in context.Hotels
                   where hotel.Owner == user
                   select new HotelDTO()
                   {
                       Id = hotel.Id,
                       Name = hotel.Name,
                       CurrencyFormat = hotel.Currency.FormattingString,
                       DownPaymentPercentage = hotel.DownPaymentPercentage
                   };
        }
    }
}
