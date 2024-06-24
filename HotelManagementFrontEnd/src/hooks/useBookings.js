import { useEffect, useState } from "react";
import { axios_base } from "../util/axios";

export default (token, hotelId, roomId) => {
    const [bookings, setBookings] = useState([]);

    useEffect(() => {
        refresh()
      }, [])

    const refresh = async () => {
        const {data} = await axios_base.get(`hotels/${hotelId}/rooms/${roomId}/bookings`, {headers: {authorization: token}})
        setBookings(data)
    }

    return {bookings, refresh}
}
