using HotelManagementAPI.Models;
using HotelManagementAPI.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementAPI.DataInterfaces
{
    public interface IHotelCodeStore
    {
        void AcceptInvite(string codeId);
        string Add(HotelCodeCreateDTO hotelCodeDTO, string code, User user);
        IActionResult Delete(string id);
        HotelCode? GetByEmployeeHotel(string employeeEmail, int hotelId);
        HotelCode? GetById(string id);
        IActionResult GetInvites();
        IEnumerable<HotelCodeReceivedDTO> GetReceivedInvites(User user);
        IEnumerable<HotelCodeSentDTO> GetSentInvites(User user);
        void RejectInvite(string codeId);
        IActionResult RespondToInvite(RespondToInviteDTO inviteResponse, string codeId);
    }
}