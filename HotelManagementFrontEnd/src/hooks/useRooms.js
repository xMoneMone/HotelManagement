import { useEffect, useState } from "react";
import { axios_base } from "../util/axios";

export default (token, hotelId) => {
    const [rooms, setRooms] = useState([]);

    useEffect(() => {
        refresh()
      }, [])

    const refresh = async () => {
        const {data} = await axios_base.get(`hotels/${hotelId}/rooms`, {headers: {authorization: token}})
        setRooms(data)
    }

    return {rooms, refresh}
}
