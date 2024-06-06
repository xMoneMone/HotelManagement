using HotelManagementAPI.Models.DTO;
using HotelManagementAPI.Models;
using HotelManagementAPI.Util;
using HotelManagementAPI.DataInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementAPI.Data
{
    public class HotelCodeStore(HotelManagementContext _context, IUserStore userStore, IHotelStore hotelStore, IUserHotelStore userHotelStore,
        IHotelCodeStatusStore hotelCodeStatusStore) : IHotelCodeStore
    {
        private readonly HotelManagementContext context = _context;
        private readonly IUserStore userStore = userStore;
        private readonly IHotelStore hotelStore = hotelStore;
        private readonly IUserHotelStore userHotelStore = userHotelStore;
        private readonly IHotelCodeStatusStore hotelCodeStatusesStore = hotelCodeStatusStore;

        public IActionResult Add(HotelCodeCreateDTO hotelCodeDTO)
        {
            var user = userStore.GetCurrentUser();
            var employee = userStore.GetByEmail(hotelCodeDTO.UserEmail);
            var hotel = hotelStore.GetById(hotelCodeDTO.HotelId);
            var connection = userHotelStore.GetByHotelEmployee(hotel?.Id, employee?.Id);

            var error = InvitationValidators.InviteEmployeeValidator(user, employee, hotel, connection);

            if (error != null)
            {
                return error;
            }

            string code = CodeGenerator.GenerateCode();

            context.Add(new HotelCode
            {
                UserId = employee.Id,
                Code = code,
                HotelId = hotelCodeDTO.HotelId,
                StatusId = 1,
                SenderId = user.Id
            });

            context.SaveChanges();

            return new OkObjectResult(code);
        }

        public IActionResult RespondToInvite(RespondToInviteDTO inviteResponse, string codeId)
        {
            var user = userStore.GetCurrentUser();
            var code = GetById(codeId);

            var error = InvitationValidators.RespondToInvitationValidator(user, code);

            if (error != null)
            {
                return error;
            }

            if (inviteResponse.Accept)
            {
                AcceptInvite(codeId);
                return new OkObjectResult("Invite accepted.");
            }
            else
            {
                RejectInvite(codeId);
                return new OkObjectResult("Invite rejected.");
            }
        }

        public void AcceptInvite(string codeId)
        {
            var code = GetById(codeId);
            code.StatusId = 2;
            userHotelStore.Add(code);
            context.SaveChanges();
        }

        public void RejectInvite(string codeId)
        {
            var code = GetById(codeId);
            code.StatusId = 3;
            context.SaveChanges();
        }

        public IActionResult Delete(string id)
        {
            var user = userStore.GetCurrentUser();
            var code = GetById(id);
            var hotel = hotelStore.GetById(code?.HotelId);

            var error = InvitationValidators.DeleteInviteValidator(user.Id, code, hotel.OwnerId);

            if (error != null)
            {
                return error;
            }

            context.HotelCodes.Remove(code);
            context.SaveChanges();
            return new OkObjectResult("Invite deleted.");
        }

        public HotelCode? GetById(string id)
        {
            return (from code in context.HotelCodes
                    where id == code.Code
                    select code)
                   .FirstOrDefault();
        }

        public HotelCode? GetByEmployeeHotel(string employeeEmail, int hotelId)
        {
            var employee = userStore.GetByEmail(employeeEmail);
            return (from code in context.HotelCodes
                    where employee.Id == code.UserId && hotelId == code.HotelId
                    select code)
                   .FirstOrDefault();
        }

        public IActionResult GetInvites()
        {
            var user = userStore.GetCurrentUser();

            if (user?.AccountTypeId == 1)
            {
                return new OkObjectResult(GetSentInvites(user));
            }
            else if (user?.AccountTypeId == 2)
            {
                return new OkObjectResult(GetReceivedInvites(user));
            }

            return new BadRequestObjectResult("User does not exist.");
        }

        public IEnumerable<HotelCodeSentDTO> GetSentInvites(User user)
        {
            return from code in context.HotelCodes
                   where code.SenderId == user.Id
                   select new HotelCodeSentDTO
                   {
                       Code = code.Code,
                       HotelName = hotelStore.GetById(code.HotelId).Name,
                       UserEmail = userStore.GetById(code.UserId).Email,
                       Status = hotelCodeStatusStore.GetById(code.StatusId).Status
                   };
        }

        public IEnumerable<HotelCodeReceivedDTO> GetReceivedInvites(User user)
        {
            return from code in context.HotelCodes
                   where code.UserId == user.Id
                   select new HotelCodeReceivedDTO
                   {
                       Code = code.Code,
                       HotelName = hotelStore.GetById(code.HotelId).Name,
                       OwnerEmail = userStore.GetById(code.SenderId).Email,
                   };
        }
    }
}
