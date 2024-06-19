import { useEffect, useState } from "react";
import { axios_base } from "../util/axios";

export default (token, hotelId, dateRange) => {
    const [rooms, setRooms] = useState([]);

    useEffect(() => {
        refresh(dateRange)
      }, [])

    const refresh = async (date) => {
        const {data} = await axios_base.get(`hotels/${hotelId}/rooms${date}`, {headers: {authorization: token}})
        setRooms(data)
    }

    return {rooms, refresh}
}
