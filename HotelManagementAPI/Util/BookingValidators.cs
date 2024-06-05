﻿using HotelManagementAPI.Data;
using HotelManagementAPI.Models;
using HotelManagementAPI.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementAPI.Util
{
    public class BookingValidators
    {
        public static IActionResult? GetBookingsValidator(User user, Room? room)
        {
            if (room == null)
            {
                return new NotFoundObjectResult("Room does not exist.");
            }

            var hotelOwnerId = HotelStore.GetById(room.HotelId).OwnerId;

            if (hotelOwnerId != user.Id && !Validators.EmployeeWorksAtHotel(room.HotelId, user.Id))
            {
                return new UnauthorizedObjectResult("You do not have permission to see this resource.");
            }

            return null;
        }

        public static IActionResult? CreateBookingValidator(User user, BookingCreateDTO bookingDTO, Room? room, Hotel? hotel)
        {
            if (room == null)
            {
                return new NotFoundObjectResult("Room does not exist.");
            }

            if (hotel.OwnerId != user.Id || Validators.EmployeeWorksAtHotel(hotel.OwnerId, user.Id))
            {
                return new UnauthorizedObjectResult("You cannot make bookings at this hotel.");
            }

            if (bookingDTO.FirstName.Length <= 0 || bookingDTO.FirstName.Length > 50)
            {
                return new BadRequestObjectResult("First name number must be below 50 characters.");
            }

            if (bookingDTO.LastName.Length <= 0 || bookingDTO.LastName.Length > 50)
            {
                return new BadRequestObjectResult("First name number must be below 50 characters.");
            }

            if (bookingDTO.DownPaymentPrice < 0)
            {
                return new BadRequestObjectResult("Down payment price cannot be a negative number.");
            }

            if (bookingDTO.FullPaymentPrice < 0)
            {
                return new BadRequestObjectResult("Full payment price cannot be a negative number.");
            }

            if (bookingDTO.Notes != null && bookingDTO.Notes.Length > 300)
            {
                return new BadRequestObjectResult("Room notes must not exceed 300 characters.");
            }

            return null;
        }

        public static IActionResult? EditBookingValidator(User user, BookingCreateDTO bookingDTO, Booking? booking, Room? room, Hotel? hotel)
        {
            if (booking == null)
            {
                return new NotFoundObjectResult("Booking does not exist.");
            }

            return CreateBookingValidator(user, bookingDTO, room, hotel);
        }


        public static IActionResult? GetBookingByIdValidator(User user, Booking? booking, Room? room, Hotel? hotel)
        {
            if (booking == null)
            {
                return new BadRequestObjectResult("Booking does not exist.");
            }

            if (hotel.OwnerId != user.Id && Validators.EmployeeWorksAtHotel(hotel.Id, user.Id))
            {
                return new UnauthorizedObjectResult("You do not have permission to see this resource.");
            }

            return null;
        }

        public static IActionResult? DeleteBookingValidator(User user, Booking booking, int hotelOwnerId)
        {
            if (booking == null)
            {
                return new BadRequestObjectResult("Booking does not exist.");
            }

            if (hotelOwnerId != user.Id)
            {
                return new UnauthorizedObjectResult("You do not have permission to delete this resource.");
            }

            return null;
        }
    }
}
