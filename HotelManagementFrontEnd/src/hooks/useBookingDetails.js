import { useEffect, useState } from "react";
import { axios_base } from "../util/axios";

export default (token, hotelId, roomId, bookingId) => {
    const [booking, setBooking] = useState([]);

    useEffect(() => {
        refresh()
      }, [])

    const refresh = async () => {
        const {data} = await axios_base.get(`hotels/${hotelId}/rooms/${roomId}/bookings/${bookingId}`, {headers: {authorization: token}})
        setBooking(data)
    }

    return {booking, refresh}
}
