using HotelManagementAPI.Models;
using HotelManagementAPI.Models.DTO;

namespace HotelManagementAPI.Data
{
    public class BookingStore : DataStore
    {
        public static void Add(BookingCreateDTO bookingDTO, int roomId)
        {
            context.Add(new Booking
            {
                FirstName = bookingDTO.FirstName,
                LastName = bookingDTO.LastName,
                StartDate = bookingDTO.StartDate,
                EndDate = bookingDTO.EndDate,
                DownPaymentPaid = bookingDTO.DownPaymentPaid,
                FullPaymentPaid = bookingDTO.FullPaymentPaid,
                DownPaymentPrice = bookingDTO.DownPaymentPrice,
                FullPaymentPrice = bookingDTO.FullPaymentPrice,
                Notes = bookingDTO.Notes,
                RoomId = roomId
            });
            context.SaveChanges();
        }

        public static void Edit(int id, BookingCreateDTO bookingDTO)
        {
            var booking = GetById(id);
            booking.FirstName = bookingDTO.FirstName;
            booking.LastName = bookingDTO.LastName;
            booking.StartDate = bookingDTO.StartDate;
            booking.EndDate = bookingDTO.EndDate;
            booking.DownPaymentPaid = bookingDTO.DownPaymentPaid;
            booking.FullPaymentPaid = bookingDTO.FullPaymentPaid;
            booking.DownPaymentPrice = bookingDTO.DownPaymentPrice;
            booking.FullPaymentPrice = bookingDTO.FullPaymentPrice;
            booking.Notes = bookingDTO.Notes;
            context.SaveChanges();
        }

        public static void Delete(int id)
        {
            var booking = GetById(id);
            context.Bookings.Remove(booking);
            context.SaveChanges();
        }

        public static Booking? GetById(int id)
        {
            return (from booking in context.Bookings
                    where id == booking.Id
                    select booking)
                   .FirstOrDefault();
        }

        public static IEnumerable<Booking> All()
        {
            return from booking in context.Bookings
                   select booking;
        }

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
