using HotelManagementAPI.Data;
using HotelManagementAPI.Models;
using HotelManagementAPI.Models.DTO;
using HotelManagementAPI.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementAPI.Controllers
{
    [Route("hotels"), Authorize]
    [ApiController]
    public class HotelsController : ControllerBase
    {
        [HttpGet, Authorize]
        public IEnumerable<HotelDTO> GetHotels()
        {
            var user = JwtDecoder.GetUser(User.Claims, UserStore.context);
            return HotelStore.GetUserHotels(user);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpDelete("{hotelId}/employees/{employeeId}"), Authorize]
        public IActionResult RemoveHotelEmployee(int hotelId, int employeeId)
        {
            var user = JwtDecoder.GetUser(User.Claims, UserStore.context);
            var hotel = HotelStore.context.Hotels.FirstOrDefault(x => x.Id == hotelId);
            var employee = HotelStore.context.Users.FirstOrDefault(x => x.Id == employeeId);

            var error = HotelValidators.RemoveHotelEmployeeValidator(user, employee, hotel);

            if (error != null)
            {
                return error;
            }

            var userHotelConnection = HotelStore.context.UsersHotels.FirstOrDefault(x => x.UserId == employee.Id && x.HotelId == hotel.Id);

            HotelStore.context.UsersHotels.Remove(userHotelConnection);
            HotelStore.context.SaveChanges();
            return Ok("Employee removed from hotel.");
        }

        [HttpPost, Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public IActionResult CreateHotel([FromBody] HotelCreateDTO hotelDTO)
        {
            var user = JwtDecoder.GetUser(User.Claims, UserStore.context);

            var error = HotelValidators.CreateHotelValidator(hotelDTO);

            if (error != null)
            {
                return error;
            }

            UserStore.context.Hotels.Add(new Hotel
            {
                Name = hotelDTO.Name,
                CurrencyId = Validators.ValidateMultipleChoice(HotelStore.context.Currencies, hotelDTO.CurrencyId),
                DownPaymentPercentage = hotelDTO.DownPaymentPercentage,
                OwnerId = user.Id
            });

            UserStore.context.SaveChanges();
            return Ok(hotelDTO);
        }

        [HttpPut("{id:int}"), Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult EditHotel([FromBody] HotelCreateDTO hotelDTO, int id)
        {
            var user = JwtDecoder.GetUser(User.Claims, UserStore.context);
            var hotel = HotelStore.context.Hotels.FirstOrDefault(x => x.Id == id);

            var error = HotelValidators.EditHotelValidator(hotelDTO, hotel, user);

            if (error != null)
            {
                return error;
            }

            hotel.Name = hotelDTO.Name;
            hotel.CurrencyId = Validators.ValidateMultipleChoice(HotelStore.context.Currencies, hotelDTO.CurrencyId);
            hotel.DownPaymentPercentage = hotelDTO.DownPaymentPercentage;

            HotelStore.context.SaveChanges();
            return Ok(hotelDTO);
        }


        [HttpDelete("{id:int}"), Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteHotel(int id)
        {
            var user = JwtDecoder.GetUser(User.Claims, UserStore.context);
            var hotel = HotelStore.context.Hotels.FirstOrDefault(x => x.Id == id);

            var error = HotelValidators.DeleteHotelValidator(hotel, user);

            if (error != null)
            {
                return error;
            }

            HotelStore.context.Hotels.Remove(hotel);
            HotelStore.context.SaveChanges();
            return Ok("Hotel has been deleted.");
        }
    }
}   
