import { useContext } from "react";
import useBookingDetails from "./hooks/useBookingDetails";
import { UserContext } from "./App";
import { useParams } from "react-router-dom";

export default function BookingDetails() {
    const [token, setToken] = useContext(UserContext)   
    const {hotelId, roomId, bookingId} = useParams()
    const {booking} = useBookingDetails(token, hotelId, roomId, bookingId);
    console.log(booking) 

    
}