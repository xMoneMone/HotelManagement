using HotelManagementAPI.DataInterfaces;
using HotelManagementAPI.Models;
using HotelManagementAPI.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing;

namespace HotelManagementAPI.Data
{
    public class ColorStore(HotelManagementContext _context) : IColorStore
    {
        private readonly HotelManagementContext context = _context;

        public async Task<IActionResult> GetColors()
        {
            return new OkObjectResult(await (from color in context.Colors
                                             orderby color.Id
                                             select new ColorDTO
                                             {
                                                 Id = color.Id,
                                                 Color1 = color.Color1,
                                             })
                   .ToListAsync());
        }

        public async Task<string?> GetColorById(int id)
        {
            return await (from color in context.Colors
                                             where color.Id == id
                                             select color.Color1)
                   .FirstOrDefaultAsync();
        }
    }
}
