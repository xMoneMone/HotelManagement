import { useEffect, useState } from "react";
import { axios_base } from "../util/axios";

export default (token, hotelId, roomId) => {
    const [room, setRoom] = useState([]);

    useEffect(() => {
        refresh()
      }, [])

    const refresh = async () => {
        const {data} = await axios_base.get(`hotels/${hotelId}/rooms/${roomId}`, {headers: {authorization: token}})
        setRoom(data)
    }

    return {room, refresh}
}
