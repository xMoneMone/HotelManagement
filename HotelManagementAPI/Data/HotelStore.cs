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

        public static IEnumerable<HotelCodeSentDTO> GetSentInvites(User user)
        {
            return from code in context.HotelCodes
                   where code.SenderId == user.Id
                   select new HotelCodeSentDTO
                   {
                       HotelName = context.Hotels.FirstOrDefault(x => x.Id == code.HotelId).Name,
                       UserEmail = context.Users.FirstOrDefault(x => x.Id == code.UserId).Email,
                       Status = context.HotelCodeStatuses.FirstOrDefault(x => x.Id == code.StatusId).Status
                   };
        }

        public static IEnumerable<HotelCodeReceivedDTO> GetReceivedInvites(User user)
        {
            return from code in context.HotelCodes
                   where code.UserId == user.Id
                   select new HotelCodeReceivedDTO
                   {
                       HotelName = context.Hotels.FirstOrDefault(x => x.Id == code.HotelId).Name,
                       OwnerEmail = context.Users.FirstOrDefault(x => x.Id == code.SenderId).Email,
                   };
        }

        public static int[] GetHotelEmployeesIds(int hotelId)
        {
            return UserStore.context.UsersHotels.Where(x => x.HotelId == hotelId).Select(x => x.UserId).ToArray();
        }
    }
}
