using HotelManagementAPI.Models.DTO;
using HotelManagementAPI.Models;
using HotelManagementAPI.Util;
using HotelManagementAPI.DataInterfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementAPI.Data
{
    public class HotelCodeStore(HotelManagementContext _context, IUserStore userStore, IHotelStore hotelStore, IUserHotelStore userHotelStore) : IHotelCodeStore
    {
        private readonly HotelManagementContext context = _context;
        private readonly IUserStore userStore = userStore;
        private readonly IHotelStore hotelStore = hotelStore;
        private readonly IUserHotelStore userHotelStore = userHotelStore;

        public async Task<IActionResult> Add(HotelCodeCreateDTO hotelCodeDTO)
        {
            var user = await userStore.GetCurrentUser();
            var employee = await userStore.GetByEmail(hotelCodeDTO.UserEmail);
            var hotel = await hotelStore.GetById(hotelCodeDTO.HotelId);
            var connection = await userHotelStore.GetByHotelEmployee(hotel?.Id, employee?.Id);

            var error = InvitationValidators.InviteEmployeeValidator(user, employee, hotel, connection);

            if (error != null)
            {
                return error;
            }

            string code = CodeGenerator.GenerateCode();

            await context.AddAsync(new HotelCode
            {
                UserId = employee.Id,
                Code = code,
                HotelId = hotelCodeDTO.HotelId,
                StatusId = 1,
                SenderId = user.Id
            });

            await context.SaveChangesAsync();

            return new OkObjectResult(code);
        }

        public async Task<IActionResult> RespondToInvite(RespondToInviteDTO inviteResponse, string codeId)
        {
            var user = await userStore.GetCurrentUser();
            var code = await GetById(codeId);

            var error = InvitationValidators.RespondToInvitationValidator(user, code);

            if (error != null)
            {
                return error;
            }

            if (inviteResponse.Accept)
            {
                await AcceptInvite(codeId);
                return new OkObjectResult("Invite accepted.");
            }
            else
            {
                await RejectInvite(codeId);
                return new OkObjectResult("Invite rejected.");
            }
        }

        public async Task<bool> AcceptInvite(string codeId)
        {
            var code = await GetById(codeId);
            code.StatusId = 2;
            await userHotelStore.Add(code);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RejectInvite(string codeId)
        {
            var code = await GetById(codeId);
            code.StatusId = 3;
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<IActionResult> Delete(string id)
        {
            var user = await userStore.GetCurrentUser();
            var code = await GetById(id);
            var hotel = await hotelStore.GetById(code?.HotelId);

            var error = InvitationValidators.DeleteInviteValidator(user.Id, code, hotel.OwnerId);

            if (error != null)
            {
                return error;
            }

            context.HotelCodes.Remove(code);
            await context.SaveChangesAsync();
            return new ObjectResult("") { StatusCode = 204 };
        }

        public async Task<HotelCode?> GetById(string id)
        {
            return await (from code in context.HotelCodes
                          where id == code.Code
                          select code)
                   .FirstOrDefaultAsync();
        }

        public async Task<HotelCode?> GetByEmployeeHotel(string employeeEmail, int hotelId)
        {
            var employee = userStore.GetByEmail(employeeEmail);
            return await (from code in context.HotelCodes
                          where employee.Id == code.UserId && hotelId == code.HotelId
                          select code)
                   .FirstOrDefaultAsync();
        }

        public async Task<IActionResult> GetInvites()
        {
            var user = await userStore.GetCurrentUser();

            if (user?.AccountTypeId == 1)
            {
                return new OkObjectResult(await GetSentInvites(user));
            }
            else if (user?.AccountTypeId == 2)
            {
                return new OkObjectResult(await GetReceivedInvites(user));
            }

            return new BadRequestObjectResult("User does not exist.");
        }

        public async Task<IEnumerable<HotelCodeSentDTO>> GetSentInvites(User user)
        {
            return await (from code in context.HotelCodes
                          where code.SenderId == user.Id
                          join hotel in context.Hotels on code.HotelId equals hotel.Id
                          join employee in context.Users on code.UserId equals employee.Id
                          join status in context.HotelCodeStatuses on code.StatusId equals status.Id
                          select new HotelCodeSentDTO
                          {
                              Code = code.Code,
                              HotelName = hotel.Name,
                              UserEmail = employee.Email,
                              Status = status.Status
                          }).ToListAsync();
        }

        public async Task<IEnumerable<HotelCodeReceivedDTO>> GetReceivedInvites(User user)
        {
            return await (from code in context.HotelCodes
                          where code.UserId == user.Id
                          join hotel in context.Hotels on code.HotelId equals hotel.Id
                          join owner in context.Users on code.SenderId equals owner.Id
                          select new HotelCodeReceivedDTO
                          {
                              Code = code.Code,
                              HotelName = hotel.Name,
                              OwnerEmail = owner.Email,
                          }).ToListAsync();
        }
    }
}
