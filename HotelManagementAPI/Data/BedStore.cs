using HotelManagementAPI.DataInterfaces;
using HotelManagementAPI.Models;
using HotelManagementAPI.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementAPI.Data
{
    public class BedStore(HotelManagementContext context) : IBedStore
    {
        private readonly HotelManagementContext context = context;

        public async Task<List<BedDTO>> GetRoomBeds(int roomId)
        {
            return await (from roombed in context.RoomsBeds
                          join bed in context.Beds on roombed.BedId equals bed.Id
                          where roombed.RoomId == roomId
                          select new BedDTO
                          {
                              BedType = bed.BedType,
                              Capacity = bed.Capacity
                          }).ToListAsync();
        }
    }
}
