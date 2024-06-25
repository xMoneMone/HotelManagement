import { useContext, useEffect } from "react";
import useBookingDetails from "./hooks/useBookingDetails";
import { UserContext } from "./App";
import useHotelCurrency from "./hooks/useHotelCurrency"
import { useParams } from "react-router-dom";
import './css/bookingdetails.css'

export default function BookingDetails() {
    const [token, setToken] = useContext(UserContext)   
    const {hotelId, roomId, bookingId} = useParams()
    const {booking} = useBookingDetails(token, hotelId, roomId, bookingId);
    const {currency} = useHotelCurrency(token, hotelId)
    console.log(currency) 

    useEffect(() => {
        document.title = `Booking Details | HMT`;
    }, []);

    return <div className="booking-details-page">
            <div className="booking-details">
                    <div className="line booking-details-line"></div>
                    <h1 className="booking-details-name">
                        {booking.firstName + " " + booking.lastName}
                    </h1>
                    <div className="booking-details-date">
                        <div>
                            Start date:
                            {booking.startDate && booking.startDate.substring(0, 10)}
                        </div>
                        <div>
                            End date:
                            {booking.startDate && booking.endDate.substring(0, 10)}
                        </div>
                    </div>
                    <div className="booking-details-payment">
                        <div className="booking-details-payment-down">
                            <div>
                                Down payment price:
                                {currency && booking.downPaymentPrice && currency.replace("*", booking.downPaymentPrice.toString())}
                            </div>
                            <div>
                                Down payment:
                                {booking.downPaid && <svg className="icon booking-payment-icon" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512"><path d="M256 512A256 256 0 1 0 256 0a256 256 0 1 0 0 512zM369 209L241 337c-9.4 9.4-24.6 9.4-33.9 0l-64-64c-9.4-9.4-9.4-24.6 0-33.9s24.6-9.4 33.9 0l47 47L335 175c9.4-9.4 24.6-9.4 33.9 0s9.4 24.6 0 33.9z"/></svg>}
                                {!booking.downPaid && <svg className="icon booking-payment-icon" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512"><path d="M256 512A256 256 0 1 0 256 0a256 256 0 1 0 0 512zM175 175c9.4-9.4 24.6-9.4 33.9 0l47 47 47-47c9.4-9.4 24.6-9.4 33.9 0s9.4 24.6 0 33.9l-47 47 47 47c9.4 9.4 9.4 24.6 0 33.9s-24.6 9.4-33.9 0l-47-47-47 47c-9.4 9.4-24.6 9.4-33.9 0s-9.4-24.6 0-33.9l47-47-47-47c-9.4-9.4-9.4-24.6 0-33.9z"/></svg>}
                            </div>
                        </div>
                        <div className="booking-details-payment-full">
                            <div>
                                Full payment price:
                                {currency && booking.fullPaymentPrice && currency.replace("*", booking.fullPaymentPrice.toString())}
                            </div>
                            <div>
                                Full payment:
                                {booking.fullPaid && <svg className="icon booking-payment-icon" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512"><path d="M256 512A256 256 0 1 0 256 0a256 256 0 1 0 0 512zM369 209L241 337c-9.4 9.4-24.6 9.4-33.9 0l-64-64c-9.4-9.4-9.4-24.6 0-33.9s24.6-9.4 33.9 0l47 47L335 175c9.4-9.4 24.6-9.4 33.9 0s9.4 24.6 0 33.9z"/></svg>}
                                {!booking.fullPaid && <svg className="icon booking-payment-icon" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512"><path d="M256 512A256 256 0 1 0 256 0a256 256 0 1 0 0 512zM175 175c9.4-9.4 24.6-9.4 33.9 0l47 47 47-47c9.4-9.4 24.6-9.4 33.9 0s9.4 24.6 0 33.9l-47 47 47 47c9.4 9.4 9.4 24.6 0 33.9s-24.6 9.4-33.9 0l-47-47-47 47c-9.4 9.4-24.6 9.4-33.9 0s-9.4-24.6 0-33.9l47-47-47-47c-9.4-9.4-9.4-24.6 0-33.9z"/></svg>}
                            </div>
                        </div>
                    </div>
                    <div className="line booking-details-line"></div>
                </div>
            </div>
}