using HotelManagementAPI.Models.DTO;
using HotelManagementAPI.Models;
using HotelManagementAPI.Util;

namespace HotelManagementAPI.Data
{
    public class HotelCodeStore : DataStore
    {
        public static string Add(HotelCodeCreateDTO hotelCodeDTO, string code, User user)
        {
            var employee = UserStore.GetByEmail(hotelCodeDTO.UserEmail);

            context.Add(new HotelCode
            {
                UserId = employee.Id,
                Code = code,
                HotelId = hotelCodeDTO.HotelId,
                StatusId = 1,
                SenderId = user.Id
            });

            context.SaveChanges();

            return code;
        }

        public static void AcceptInvite(string codeId)
        {
            var code = GetById(codeId);
            code.StatusId = 2;
            UserHotelStore.Add(code);
            context.SaveChanges();
        }

        public static void RejectInvite(string codeId)
        {
            var code = GetById(codeId);
            code.StatusId = 3;
            context.SaveChanges();
        }

        public static void Delete(string id)
        {
            var code = GetById(id);
            context.HotelCodes.Remove(code);
            context.SaveChanges();
        }

        public static HotelCode? GetById(string id)
        {
            return (from code in context.HotelCodes
                    where id == code.Code
                    select code)
                   .FirstOrDefault();
        }

        public static HotelCode? GetByEmployeeHotel(string employeeEmail, int hotelId)
        {
            var employee = UserStore.GetByEmail(employeeEmail);
            return (from code in context.HotelCodes
                    where employee.Id == code.UserId && hotelId == code.HotelId
                    select code)
                   .FirstOrDefault();
        }

        public static IEnumerable<HotelCode> All()
        {
            return from code in context.HotelCodes
                   select code;
        }
        
        public static IEnumerable<HotelCodeSentDTO> GetSentInvites(User user)
        {
            return from code in context.HotelCodes
                   where code.SenderId == user.Id
                   select new HotelCodeSentDTO
                   {
                       Code = code.Code,
                       HotelName = HotelStore.GetById(code.HotelId).Name,
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
                       HotelName = HotelStore.GetById(code.HotelId).Name,
                       OwnerEmail = context.Users.FirstOrDefault(x => x.Id == code.SenderId).Email,
                   };
        }
    }
}
