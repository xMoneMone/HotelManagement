using HotelManagementAPI.Models;
using HotelManagementAPI.Models.DTO;

namespace HotelManagementAPI.Data
{
    public class BookingStore
    {
        public static HotelManagementContext context = new HotelManagementContext();

        public static IEnumerable<BookingDTO> GetBookings(Room room)
        {
            return from booking in context.Bookings
                   where booking.RoomId == room.Id
                   select new BookingDTO
                   {
                       Id = booking.Id,
                       FirstName = booking.FirstName,
                       LastName = booking.LastName,
                       StartDate = booking.StartDate,
                       EndDate = booking.EndDate,
                       DownPaymentPaid = booking.DownPaymentPaid,
                       FullPaymentPaid = booking.FullPaymentPaid,
                       DownPaymentPrice = booking.DownPaymentPrice,
                       FullPaymentPrice = booking.FullPaymentPrice,
                       Notes = booking.Notes,
                       RoomId = room.Id
                   };
        }
    }
}
