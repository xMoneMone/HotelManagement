using HotelManagementAPI.Models.DTO;

namespace HotelManagementAPI.DataInterfaces
{
    public interface IBedStore
    {
        Task<List<BedDTO>> GetRoomBeds(int roomId);
    }
}