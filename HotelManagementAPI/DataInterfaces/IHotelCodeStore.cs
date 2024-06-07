using HotelManagementAPI.Models;
using HotelManagementAPI.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementAPI.DataInterfaces
{
    public interface IHotelCodeStore
    {
        void AcceptInvite(string codeId);
        Task<IActionResult> Add(HotelCodeCreateDTO hotelCodeDTO);
        Task<IActionResult> Delete(string id);
        Task<HotelCode?> GetByEmployeeHotel(string employeeEmail, int hotelId);
        Task<HotelCode?> GetById(string id);
        Task<IActionResult> GetInvites();
        Task<IEnumerable<HotelCodeReceivedDTO>> GetReceivedInvites(User user);
        Task<IEnumerable<HotelCodeSentDTO>> GetSentInvites(User user);
        void RejectInvite(string codeId);
        Task<IActionResult> RespondToInvite(RespondToInviteDTO inviteResponse, string codeId);
    }
}