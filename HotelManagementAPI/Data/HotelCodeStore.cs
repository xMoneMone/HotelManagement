using HotelManagementAPI.Models.DTO;
using HotelManagementAPI.Models;

namespace HotelManagementAPI.Data
{
    public class HotelCodeStore : DataStore
    {
        public static IEnumerable<HotelCodeSentDTO> GetSentInvites(User user)
        {
            return from code in context.HotelCodes
                   where code.SenderId == user.Id
                   select new HotelCodeSentDTO
                   {
                       Code = code.Code,
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
                       Code = code.Code,
                       HotelName = context.Hotels.FirstOrDefault(x => x.Id == code.HotelId).Name,
                       OwnerEmail = context.Users.FirstOrDefault(x => x.Id == code.SenderId).Email,
                   };
        }
    }
}
